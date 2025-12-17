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
        public IActionResult Index(string search, string sort = "date", int page = 1)
        {
            int perPage = 5;   // câte melodii afișezi pe pagină
            var songsQuery = db.Songs
                .Include(s => s.User)
                .Include(s => s.SongTags).ThenInclude(st => st.Tag)
                .Include(s => s.Comms)
                .AsQueryable();

            // search bar
            ViewBag.Search = search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                // Căutare în titlu + artist
                var idsSongs = db.Songs
                .Include(s => s.User)
                .Where(s =>
                    s.Title.ToLower().Contains(search) ||
                    (s.User != null && s.User.Name.ToLower().Contains(search)))
                .Select(s => s.Id)
                .ToList();



                // Căutare în tag-uri
                var idsTags = db.SongTags
                    .Where(st => st.Tag.Name.ToLower().Contains(search))
                    .Select(st => st.SongId)
                    .ToList();

                // Căutare în comentarii
                var idsComments = db.Comms
                    .Where(c => c.Text.ToLower().Contains(search))
                    .Select(c => c.SongId)
                    .ToList();

                var mergedIds = idsSongs
                    .Union(idsTags)
                    .Union(idsComments)
                    .Distinct()
                    .ToList();

                songsQuery = songsQuery.Where(s => mergedIds.Contains(s.Id));
            }
 
            //PAGINAȚIE  
            int totalItems = songsQuery.Count();
            int lastPage = (int)Math.Ceiling((double)totalItems / perPage);

            int offset = (page - 1) * perPage;

            // SORTARE
            switch (sort)
            {
                case "likes":
                    songsQuery = songsQuery
                        .OrderByDescending(s =>
                            _context.SongVotes.Count(v => v.SongId == s.Id && v.IsLike == true));
                    break;

                default: // date
                    songsQuery = songsQuery
                        .OrderByDescending(s => s.DataPublicarii);
                    break;
            }

            // PAGINARE
            var songs = songsQuery
                .Skip(offset)
                .Take(perPage)
                .ToList();


            ViewBag.Songs = songs;
            ViewBag.lastPage = lastPage;
            ViewBag.Sort = sort;


            // Construirea URL-ului pentru paginare
            if (!string.IsNullOrEmpty(search))
                ViewBag.PaginationBaseUrl = $"/Songs/Index?search={search}&sort={sort}&page=";
            else
                ViewBag.PaginationBaseUrl = $"/Songs/Index?sort={sort}&page=";

            // Playlist-urile userului
            if (User.Identity.IsAuthenticated)
            {
                string userId = _userManager.GetUserId(User);
                ViewBag.Playlists = db.Playlists.Where(p => p.UserId == userId).ToList();
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

            // VALIDARE AUDIO
            if (AudioFile == null || AudioFile.Length == 0)
                ModelState.AddModelError("AudioFile", "You must upload an MP3 file.");
            else if (!AudioFile.FileName.EndsWith(".mp3"))
                ModelState.AddModelError("AudioFile", "File must be MP3.");

            if (ModelState.IsValid)
            {
                var uploadPath = Path.Combine("wwwroot/uploads/songs");
                Directory.CreateDirectory(uploadPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(AudioFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await AudioFile.CopyToAsync(stream);

                song.Url = "/uploads/songs/" + fileName;

                // TAGS
                if (SelectedTags != null)
                {
                    song.SongTags = SelectedTags
                        .Select(t => new SongTag { TagId = t })
                        .ToList();
                }

                db.Songs.Add(song);
                db.SaveChanges();

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
            ModelState.Remove("NewAudioFile");



            // Permisiuni
            if (!(song.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin")))
            {
                TempData["message"] = "Nu aveți dreptul să modificați un cântec care nu vă aparține!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
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
