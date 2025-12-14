using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;
using System.Linq;

namespace Orpheo.Controllers
{
    public class SongsController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ApplicationDbContext db = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;


        // daca scriu cu Authorize, atunci vor avea permisiuni doar tipurile de utilizatori 
        // pe care ii specific, se neglijeaza cel neinregistrat
        //[Authorize(Roles = "Admin,Artist ,User")]

        // cu AllowAnnonymous dau voie tuturor tipurilor de ut, dar si celor neinregistrati
        [AllowAnonymous]
        // afisez toate cantecele
        public IActionResult Index(string search)
        {
            var songsQuery = db.Songs
                .Include(s => s.User)
                .Include(s => s.SongTags)
                    .ThenInclude(st => st.Tag)
                .AsQueryable();

            // Dacă există o căutare -> căutăm exact după titlu
            if (!string.IsNullOrWhiteSpace(search))
            {
                songsQuery = songsQuery.Where(s => s.Title.ToLower() == search.ToLower());
            }

            var songs = songsQuery
                .OrderByDescending(s => s.Id)
                .ToList();

            // Playlist-urile userului doar dacă e logat
            if (User.Identity.IsAuthenticated)
            {
                string userId = _userManager.GetUserId(User);
                ViewBag.Playlists = db.Playlists.Where(p => p.UserId == userId).ToList();
            }

            ViewBag.Search = search;
            ViewBag.Songs = songs;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.AlertType = TempData["messageType"];
            }

            SetAccessRights();
            return View();
        }


        public async Task<IActionResult> Like(int id)
        {
            var userId = _userManager.GetUserId(User);

            // căutăm votul existent
            var vote = await _context.SongVotes
                .FirstOrDefaultAsync(v => v.SongId == id && v.UserId == userId);

            if (vote == null)
            {
                vote = new SongVote
                {
                    SongId = id,
                    UserId = userId,
                    IsLike = true
                };
                _context.SongVotes.Add(vote);
            }
            else
            {
                vote.IsLike = true;
            }

            // găsim playlistul Favorites al userului
            var favorites = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Name == "Favorites");

            // dacă userul NU are playlist Favorites → îl creăm ACUM
            if (favorites == null)
            {
                favorites = new Playlist
                {
                    Name = "Favorites",
                    UserId = userId,
                    IsPublic = false,
                    ImagePath = "/images/favorites.png",
                    PlaylistSongs = new List<PlaylistSong>()
                };

                _context.Playlists.Add(favorites);
                await _context.SaveChangesAsync();
                // acum favorites are Id -> putem adăuga melodii în el
            }

            // adăugăm melodia în playlist (dacă nu există deja)
            if (!favorites.PlaylistSongs.Any(ps => ps.SongId == id))
            {
                favorites.PlaylistSongs.Add(new PlaylistSong
                {
                    PlaylistId = favorites.Id,
                    SongId = id
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Dislike(int id)
        {
            var userId = _userManager.GetUserId(User);

            var vote = await _context.SongVotes
                .FirstOrDefaultAsync(v => v.SongId == id && v.UserId == userId);

            if (vote == null)
            {
                vote = new SongVote
                {
                    SongId = id,
                    UserId = userId,
                    IsLike = false
                };
                _context.SongVotes.Add(vote);
            }
            else
            {
                vote.IsLike = false;
            }

            // găsim playlistul Favorites
            var favorites = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Name == "Favorites");

            if (favorites != null)
            {
                var link = favorites.PlaylistSongs.FirstOrDefault(ps => ps.SongId == id);
                if (link != null)
                {
                    favorites.PlaylistSongs.Remove(link);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        //[Authorize(Roles = "Admin,Artist,User")]
        [AllowAnonymous]
        [HttpGet]
        // afisez un cantec anume
        public IActionResult Show(int id)
        {
            Song? song = db.Songs
                           .Include(s => s.User)
                           .Include(s => s.SongTags)
                               .ThenInclude(st => st.Tag)
                           .Include(s => s.Comms)
                               .ThenInclude(c => c.User)
                           .Where(s => s.Id == id)
                           .FirstOrDefault();

            if (song is null)
            {
                return NotFound();
            }

            SetAccessRights();

            return View(song);
        }

        [Authorize(Roles = "Admin,Artist,User")]
        [HttpPost]
        public IActionResult Show([FromForm] Comm comm)
        {
            comm.CreatedAt = DateTime.Now;
            comm.UserId = _userManager.GetUserId(User);

            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                db.Comms.Add(comm);
                db.SaveChanges();
                return Redirect("/Songs/Show/" + comm.SongId);
            }
            else
            {
                Song? song = db.Songs
                               .Include(s => s.User)
                               .Include(s => s.SongTags)
                                   .ThenInclude(st => st.Tag)
                               .Include(s => s.Comms)
                                   .ThenInclude(c => c.User)
                               .Where(s => s.Id == comm.SongId)
                               .FirstOrDefault();

                if (song is null)
                {
                    return NotFound();
                }

                SetAccessRights();

                return View(song);
            }
        }

        [Authorize(Roles = "Admin,Artist")]
        [HttpGet]
        public IActionResult New()
        {
            ViewBag.Tags = GetAllTags();
            Song song = new Song();
            return View(song);
        }

        [Authorize(Roles = "Admin,Artist")]
        [HttpPost]
        public async Task<IActionResult> New(Song song, int[] SelectedTags, IFormFile AudioFile)

        {
            song.UserId = _userManager.GetUserId(User);
            ModelState.Remove("UserId");
            ModelState.Remove("Url");


            song.DataPublicarii = DateTime.Now;

            // VALIDARE ARTIST EXISTENT CU ROL DE "Artist"
            var artistUser = db.Users
                .Where(u => u.Name == song.Artist)
                .FirstOrDefault();

            if (artistUser == null)
            {
                ModelState.AddModelError("Artist", "Artistul introdus nu există în baza de date.");
            }
            else
            {
                // Verificăm dacă userul are rolul "Artist"
                var artistRoleId = db.Roles
                    .Where(r => r.Name == "Artist")
                    .Select(r => r.Id)
                    .FirstOrDefault();

                bool userIsArtist = db.UserRoles
                    .Any(ur => ur.UserId == artistUser.Id && ur.RoleId == artistRoleId);

                if (!userIsArtist)
                {
                    ModelState.AddModelError("Artist", "Utilizatorul există, dar nu are rolul Artist.");
                }
            }

            if (AudioFile == null || AudioFile.Length == 0)
            {
                ModelState.AddModelError("AudioFile", "Trebuie să încărcați un fișier MP3.");
            }
            else if (!AudioFile.FileName.EndsWith(".mp3"))
            {
                ModelState.AddModelError("AudioFile", "Fișierul trebuie să fie MP3.");
            }

            if (ModelState.IsValid)
            {
                var uploadPath = Path.Combine("wwwroot/uploads/songs");
                Directory.CreateDirectory(uploadPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(AudioFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AudioFile.CopyToAsync(stream);
                }

                song.Url = "/uploads/songs/" + fileName;

                // TAG-URI
                if (SelectedTags != null && SelectedTags.Length > 0)
                {
                    song.SongTags = new List<SongTag>();
                    foreach (var tagId in SelectedTags)
                    {
                        song.SongTags.Add(new SongTag { TagId = tagId });
                    }
                }

                db.Songs.Add(song);
                db.SaveChanges();

                TempData["message"] = "Song has been added";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }

            ViewBag.Tags = GetAllTags();
            return View(song);
        }

        [Authorize(Roles = "Admin,Artist")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Song? song = db.Songs
                           .Include(s => s.SongTags)
                           .Where(s => s.Id == id)
                           .FirstOrDefault();

            if (song is null)
            {
                return NotFound();
            }

            if (song.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                ViewBag.Tags = GetAllTags();
                ViewBag.SelectedTags = song.SongTags.Select(st => st.TagId).ToList();
                return View(song);
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să modificați un cântec care nu vă aparține!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin,Artist")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Song requestSong, int[] SelectedTags, IFormFile NewAudioFile)
        {
            Song? song = db.Songs
                .Include(s => s.SongTags)
                .FirstOrDefault(s => s.Id == id);

            if (song is null)
                return NotFound();

            ModelState.Remove("UserId");
            ModelState.Remove("Url");

            requestSong.UserId = song.UserId;

            // Permisiuni
            if (!(song.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin")))
            {
                TempData["message"] = "Nu aveți dreptul să modificați un cântec care nu vă aparține!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            // VALIDARE ARTIST
            var artistUser = db.Users.FirstOrDefault(u => u.Name == requestSong.Artist);
            if (artistUser == null)
                ModelState.AddModelError("Artist", "Artistul introdus nu există în baza de date.");
            else
            {
                var artistRoleId = db.Roles.Where(r => r.Name == "Artist").Select(r => r.Id).FirstOrDefault();
                bool userIsArtist = db.UserRoles.Any(ur => ur.UserId == artistUser.Id && ur.RoleId == artistRoleId);
                if (!userIsArtist)
                    ModelState.AddModelError("Artist", "Utilizatorul există, dar nu are rolul de Artist.");
            }

            if (ModelState.IsValid)
            {
                if (NewAudioFile != null && NewAudioFile.Length > 0)
                {
                    if (!NewAudioFile.FileName.EndsWith(".mp3"))
                    {
                        ModelState.AddModelError("NewAudioFile", "Fișierul trebuie să fie MP3.");
                        ViewBag.Tags = GetAllTags();
                        return View(song);
                    }

                    // Ștergere vechi
                    if (!string.IsNullOrEmpty(song.Url))
                    {
                        var oldPath = "wwwroot" + song.Url.Replace("/", "\\");
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    // Upload nou
                    var uploadPath = Path.Combine("wwwroot/uploads/songs");
                    Directory.CreateDirectory(uploadPath);

                    var fileName = Guid.NewGuid() + Path.GetExtension(NewAudioFile.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await NewAudioFile.CopyToAsync(stream);
                    }

                    song.Url = "/uploads/songs/" + fileName;
                }

                song.Title = requestSong.Title;
                song.Artist = requestSong.Artist;

                song.SongTags.Clear();
                if (SelectedTags != null && SelectedTags.Length > 0)
                {
                    foreach (var tagId in SelectedTags)
                        song.SongTags.Add(new SongTag { SongId = song.Id, TagId = tagId });
                }

                db.SaveChanges();

                TempData["message"] = "Cântecul a fost modificat";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }

            ViewBag.Tags = GetAllTags();
            ViewBag.SelectedTags = song.SongTags.Select(t => t.TagId).ToList();
            return View(song);
        }



        [Authorize(Roles = "Admin,Artist")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Song? song = db.Songs
                           .Include(s => s.User)
                           .Where(s => s.Id == id)
                           .FirstOrDefault();

            if (song is null)
            {
                return NotFound();
            }

            if (song.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(song);
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să ștergeți un cântec care nu vă aparține!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin,Artist")]
        [HttpPost]
        public IActionResult Delete(int id, Song requestSong)
        {
            Song? song = db.Songs.Find(id);

            if (song is null)
            {
                return NotFound();
            }

            if (song.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                // Ștergere SongTags
                var songTags = db.SongTags.Where(st => st.SongId == song.Id);
                db.SongTags.RemoveRange(songTags);

                // Ștergere PlaylistSongs
                var playlistSongs = db.PlaylistSongs.Where(ps => ps.SongId == song.Id);
                db.PlaylistSongs.RemoveRange(playlistSongs);

                // Ștergere comentarii
                var comms = db.Comms.Where(c => c.SongId == song.Id);
                db.Comms.RemoveRange(comms);

                // Ștergere finală song
                db.Songs.Remove(song);
                db.SaveChanges();


                TempData["message"] = "Cântecul a fost șters";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să ștergeți un cântec care nu vă aparține!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Artist") || User.IsInRole("Admin"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.UserCurent = _userManager.GetUserId(User);
            ViewBag.IsAdmin = User.IsInRole("Admin");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllTags()
        {
            var selectList = new List<SelectListItem>();

            var tags = from tag in db.Tags
                       select tag;

            foreach (var tag in tags)
            {
                selectList.Add(new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.Name  
                });
            }

            return selectList;
        }
    }
}
