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
        private readonly ApplicationDbContext db = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        [Authorize(Roles = "Admin,Artist,User")]
        public IActionResult Index()
        {
            var songs = db.Songs
                          .Include(s => s.User)
                          .Include(s => s.SongTags)
                              .ThenInclude(st => st.Tag)
                          .OrderByDescending(s => s.Id)
                          .ToList();

            ViewBag.Songs = songs;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.AlertType = TempData["messageType"];
            }

            return View();
        }

        [Authorize(Roles = "Admin,Artist,User")]
        [HttpGet]
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
        public IActionResult New(Song song, int[] SelectedTags)
        {
            song.UserId = _userManager.GetUserId(User);
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                if (SelectedTags != null && SelectedTags.Length > 0)
                {
                    song.SongTags = new List<SongTag>();
                    foreach (var tagId in SelectedTags)
                    {
                        song.SongTags.Add(new SongTag
                        {
                            TagId = tagId
                        });
                    }
                }

                db.Songs.Add(song);
                db.SaveChanges();

                TempData["message"] = "Cântecul a fost adăugat";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Tags = GetAllTags();
                return View(song);
            }
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
        public IActionResult Edit(int id, Song requestSong, int[] SelectedTags)
        {
            Song? song = db.Songs
                           .Include(s => s.SongTags)
                           .Where(s => s.Id == id)
                           .FirstOrDefault();

            if (song is null)
            {
                return NotFound();
            }
            ModelState.Remove("UserId");
            requestSong.UserId = song.UserId;

            if (song.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    song.Title = requestSong.Title;
                    song.Artist = requestSong.Artist;
                    song.Url = requestSong.Url;

                    song.SongTags.Clear();

                    if (SelectedTags != null && SelectedTags.Length > 0)
                    {
                        foreach (var tagId in SelectedTags)
                        {
                            song.SongTags.Add(new SongTag
                            {
                                SongId = song.Id,
                                TagId = tagId
                            });
                        }
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

            TempData["message"] = "Nu aveți dreptul să modificați un cântec care nu vă aparține!";
            TempData["messageType"] = "alert-danger";
            return RedirectToAction("Index");
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
