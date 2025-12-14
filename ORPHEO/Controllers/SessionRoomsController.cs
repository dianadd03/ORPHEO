using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;

namespace Orpheo.Controllers
{
    public class SessionRoomsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ApplicationDbContext db = context;
        private readonly UserManager <ApplicationUser> _userManager = userManager;


        // afisez toate SessionRoom-urile
        [Authorize(Roles = "User,Artist,Admin")]

        public IActionResult Index()
        {
            var sessionRooms = db.SessionRooms
                .Include(sr => sr.HostUser)
                .Include(sr => sr.Playlist)
                .Include(sr => sr.SessionRoomUsers)
                    .ThenInclude(sru => sru.User)
                .OrderByDescending(sr => sr.Id)
                .ToList();

            ViewBag.SessionRooms = sessionRooms;

            if(TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.AlertType = TempData["messageType"];
            }

            return View();
        }

        [Authorize(Roles = "User,Artist,Admin")]
        [HttpGet]
        public IActionResult Show(int id)
        {
            var sessionRoom = db.SessionRooms
                .Include(sr => sr.HostUser)
                .Include(sr => sr.Playlist)
                    .ThenInclude(p => p.PlaylistSongs)
                        .ThenInclude(ps => ps.Song)
                .Include(sr => sr.SessionRoomUsers)
                    .ThenInclude(sru => sru.User)
                .FirstOrDefault(sr => sr.Id == id);

            if (sessionRoom == null)
                return NotFound();



            return View(sessionRoom);
        }

        [Authorize(Roles = "User,Artist,Admin")]
        [HttpGet]
        public IActionResult New()
        {
            ViewBag.Playlists = new SelectList(GetAllPlaylists(), "Value", "Text");
            SessionRoom sessionRoom = new SessionRoom();
            return View(sessionRoom);
        }


        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public async Task<IActionResult> New(SessionRoom sessionRoom)
        {
            sessionRoom.HostUserId = _userManager.GetUserId(User);
            ModelState.Remove("HostUserId"); // elimin erorile de validare 
                                             // HostUserId ul e setat din controller, deci poate avea erori la validare pentru ca nu e completat din from


            if (ModelState.IsValid)
            {
                db.SessionRooms.Add(sessionRoom);
                db.SaveChanges();

                TempData["message"] = "SessionRoom adaugat!";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }

            ViewBag.Playlists = GetAllPlaylists();
            return View(sessionRoom);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllPlaylists()
        {
            var selectList = new List<SelectListItem>();

            var plys = from ply in db.Playlists
                       select ply;

            foreach (var ply in plys)
            {
                selectList.Add(new SelectListItem
                {
                    Value = ply.Id.ToString(),
                    Text = ply.Name
                });
            }

            return selectList;
        }

    }
}
