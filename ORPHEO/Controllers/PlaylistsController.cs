using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;

namespace Orpheo.Controllers
{
    public class PlaylistsController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly ApplicationDbContext db = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;



        //[Authorize(Roles = "User,Artist,Admin")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var playlists = db.Playlists
                            .Where(p =>
                                p.IsPublic ||
                                (User.Identity.IsAuthenticated && p.UserId == _userManager.GetUserId(User)) ||
                                User.IsInRole("Admin")
                            )
                            .Include(p => p.User)
                            .Include(p => p.PlaylistSongs).ThenInclude(ps => ps.Song)
                            .OrderByDescending(p => p.Id)
                            .ToList();


            ViewBag.Playlists = playlists;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.AlertType = TempData["messageType"];
            }

            return View();
        }

        //[Authorize(Roles = "User,Artist,Admin")]
        [AllowAnonymous]
        public IActionResult Show(int id, string search)
        {
            Playlist? playlist = db.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs).ThenInclude(ps => ps.Song)
                .Include(p => p.SessionRooms)
                .FirstOrDefault(p => p.Id == id);
            if (!playlist.IsPublic &&
                        playlist.UserId != _userManager.GetUserId(User) &&
                        !User.IsInRole("Admin"))
            {
                TempData["message"] = "This playlist is private!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }


            if (playlist == null)
                return NotFound();


            if (TempData.ContainsKey("AddSongMessage"))
            {
                ViewBag.AddSongMessage = TempData["AddSongMessage"];
            }


            ViewBag.Search = search;
            ViewBag.FoundSong = null;

            if (!string.IsNullOrEmpty(search))
            {
                var found = playlist.PlaylistSongs
                    .FirstOrDefault(ps =>
                        ps.Song.Title.Trim().Equals(search.Trim(), StringComparison.OrdinalIgnoreCase));

                TempData["Search"] = search;
                TempData["Found"] = (found != null);

                if (found != null)
                {
                    TempData["Title"] = found.Song.Title;
                }

                return RedirectToAction("Show", new { id = id });
            }

            if (TempData.Peek("Search") != null)
            {
                ViewBag.JustSearched = true;
                ViewBag.Search = TempData["Search"].ToString();

                bool foundd = (bool)TempData["Found"];

                ViewBag.FoundSongTitle = foundd
                    ? TempData["Title"].ToString()
                    : null;
            }
            else
            {
                ViewBag.JustSearched = false;
            }

            //dropdown d emelodii
            ViewBag.Songs = GetAllSongs();

            
            SetAccessRights(playlist);

            return View(playlist);
        }



        [Authorize(Roles = "User,Artist,Admin")]
        [HttpGet]
        public IActionResult New()
        {
            Playlist playlist = new Playlist();
            return View(playlist);
        }

        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public IActionResult New(Playlist playlist, IFormFile ImageFile)
        {
            playlist.UserId = _userManager.GetUserId(User);
            ModelState.Remove("UserId");

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/playlists");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(stream);
                }

                playlist.ImagePath = "/uploads/playlists/" + fileName;
            }


            if (ModelState.IsValid)
            {
                db.Playlists.Add(playlist);
                db.SaveChanges();

                TempData["message"] = "Playlist-ul a fost creat!";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }

            return View(playlist);
        }


        [Authorize(Roles = "User,Artist,Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Playlist? playlist = db.Playlists.Find(id);

            if (playlist == null)
            {
                return NotFound();
            }

            if (playlist.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(playlist);
            }

            TempData["message"] = "Nu aveți dreptul să editați acest playlist!";
            TempData["messageType"] = "alert-danger";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Playlist requestPlaylist, IFormFile ImageFile)
        {
            Playlist? playlist = db.Playlists.Find(id);

            if (playlist == null)
                return NotFound();

            ModelState.Remove("UserId");

            if (playlist.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    playlist.Name = requestPlaylist.Name;
                    playlist.IsPublic = requestPlaylist.IsPublic;

                    if (ImageFile == null || ImageFile.Length == 0)
                    {
                        // NU schimbăm ImagePath
                    }
                    else
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/playlists");
                        Directory.CreateDirectory(uploadsFolder);

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            ImageFile.CopyTo(stream);
                        }

                        playlist.ImagePath = "/uploads/playlists/" + fileName;
                    }

                    db.SaveChanges();

                    TempData["message"] = "Playlist-ul a fost modificat!";
                    TempData["messageType"] = "alert-success";

                    return RedirectToAction("Index");
                }

                return View(requestPlaylist);
            }

            TempData["message"] = "Nu aveți dreptul să editați acest playlist!";
            TempData["messageType"] = "alert-danger";
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Playlist? playlist = db.Playlists.Find(id);

            if (playlist == null)
            {
                return NotFound();
            }

            if (playlist.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.PlaylistSongs.RemoveRange(
                                                db.PlaylistSongs.Where(ps => ps.PlaylistId == id)
                                            );
                db.Playlists.Remove(playlist);
                db.SaveChanges();

                TempData["message"] = "Playlist-ul a fost șters!";
                TempData["messageType"] = "alert-success";
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să ștergeți acest playlist!";
                TempData["messageType"] = "alert-danger";
            }

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public IActionResult AddSong(int playlistId, int songId)
        {
            Playlist? playlist = db.Playlists.Find(playlistId);

            if (playlist == null)
                return NotFound();

            if (playlist.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                TempData["AddSongMessage"] = "Nu aveți dreptul să modificați acest playlist!";
                return RedirectToAction("Show", new { id = playlistId });
            }

            bool exists = db.PlaylistSongs.Any(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (exists)
            {
                TempData["AddSongMessage"] = "Acest cântec există deja în playlist!";
                return RedirectToAction("Show", new { id = playlistId });
            }

            db.PlaylistSongs.Add(new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId
            });

            db.SaveChanges();

            TempData["AddSongMessage"] = "Cântecul a fost adăugat!";
            return RedirectToAction("Show", new { id = playlistId });
        }


        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public IActionResult RemoveSong(int playlistId, int songId)
        {
            var entry = db.PlaylistSongs
                          .FirstOrDefault(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (entry != null)
            {
                db.PlaylistSongs.Remove(entry);
                db.SaveChanges();
            }

            return Redirect("/Playlists/Show/" + playlistId);
        }


        private void SetAccessRights(Playlist playlist)
        {
            ViewBag.AfisareButoane = false;

            if (playlist.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.UserCurent = _userManager.GetUserId(User);
            ViewBag.IsAdmin = User.IsInRole("Admin");
        }

        
        [NonAction]
        public IEnumerable<SelectListItem> GetAllSongs()
        {
            var selectList = new List<SelectListItem>();

            var songs = db.Songs.OrderBy(s => s.Title);

            foreach (var song in songs)
            {
                selectList.Add(new SelectListItem
                {
                    Value = song.Id.ToString(),
                    Text = song.Title
                });
            }

            return selectList;
        }

        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public IActionResult AddSongToPlaylist(int SongId, string NewPlaylistName)
        {
            string userId = _userManager.GetUserId(User);

            // 🔥 1. Căutăm playlistul cu acest nume la user
            var playlist = db.Playlists
                .FirstOrDefault(p => p.UserId == userId &&
                                     p.Name.Trim().ToLower() == NewPlaylistName.Trim().ToLower());

            // 🔥 2. Dacă NU există → îl creăm
            if (playlist == null)
            {
                playlist = new Playlist
                {
                    Name = NewPlaylistName.Trim(),
                    UserId = userId,
                    IsPublic = false
                };

                db.Playlists.Add(playlist);
                db.SaveChanges();
            }

            // 🔥 3. Verificăm dacă melodia este deja în playlist
            bool exists = db.PlaylistSongs
                .Any(ps => ps.PlaylistId == playlist.Id && ps.SongId == SongId);

            if (!exists)
            {
                db.PlaylistSongs.Add(new PlaylistSong
                {
                    PlaylistId = playlist.Id,
                    SongId = SongId
                });

                db.SaveChanges();
            }

            TempData["message"] = "Song added to playlist!";
            TempData["messageType"] = "alert-success";

            return RedirectToAction("Index", "Songs");
        }


    }
}
