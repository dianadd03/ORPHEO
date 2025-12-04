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
                              .Include(p => p.User)
                              .Include(p => p.PlaylistSongs)
                                .ThenInclude(ps => ps.Song)
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
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                .Include(p => p.SessionRooms)
                .Where(p => p.Id == id)
                .FirstOrDefault();

            if (playlist is null)
            {
                return NotFound();
            }

            // pentru afisare in view
            ViewBag.Search = search;
            ViewBag.FoundSong = null;

            // daca am scris ceva in search, caut
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

                if(TempData.ContainsKey("Search"))
                {
                    ViewBag.JustSearched = true;
                    ViewBag.Search = TempData["Search"].ToString();

                    bool foundd = (bool)TempData["Found"];

                    if (foundd)
                        ViewBag.FoundSongTitle = TempData["Title"].ToString();
                    else
                        ViewBag.FoundSongTitle = null;
                }
                else
                {
                    ViewBag.JustSearched = false;
                }

            // dropdown de melodii
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
        public IActionResult New(Playlist playlist)
        {
            playlist.UserId = _userManager.GetUserId(User);
            ModelState.Remove("UserId");

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
        public IActionResult Edit(int id, Playlist requestPlaylist)
        {
            Playlist? playlist = db.Playlists.Find(id);

            if (playlist == null)
            {
                return NotFound();
            }
            ModelState.Remove("UserId");
            requestPlaylist.UserId = playlist.UserId;
            if (playlist.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    playlist.Name = requestPlaylist.Name;

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
                TempData["message"] = "Nu aveți dreptul să modificați acest playlist!";
                TempData["messageType"] = "alert-danger";

                return Redirect("/Playlists/Show/" + playlistId);
            }

            bool exists = db.PlaylistSongs
                            .Any(ps => ps.PlaylistId == playlistId && ps.SongId == songId);

            if (!exists)
            {
                db.PlaylistSongs.Add(new PlaylistSong
                {
                    PlaylistId = playlistId,
                    SongId = songId
                });

                db.SaveChanges();

                TempData["message"] = "Cântecul a fost adăugat!";
                TempData["messageType"] = "alert-success";
            }

            return Redirect("/Playlists/Show/" + playlistId);
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
    }
}
