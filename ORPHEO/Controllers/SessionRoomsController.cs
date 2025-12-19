using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;
using System.ComponentModel;

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
            var currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");


            IQueryable<SessionRoom> query = db.SessionRooms
                .Include(sr => sr.HostUser)
                .Include(sr => sr.Playlist)
                .Include(sr => sr.SessionRoomUsers)
                    .ThenInclude(sru => sru.User);

            if(!isAdmin)
            {
                query = query.Where(sr =>
                    sr.HostUserId == currentUserId || sr.SessionRoomUsers.Any(sru=> sru.UserId == currentUserId)                    
                );
            }

            var sessionRooms = query
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
            var currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

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

            if (!isAdmin)
            {
                bool hasAccess =
                    sessionRoom.HostUserId == currentUserId ||
                    sessionRoom.SessionRoomUsers.Any(sru => sru.UserId == currentUserId);

                if (!hasAccess)
                    return Forbid();
            }


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

        [Authorize(Roles ="User, Artist,Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            var room = db.SessionRooms
                .Include(sr => sr.SessionRoomUsers)
                    .ThenInclude(sru => sru.User)
                .FirstOrDefault(sr => sr.Id == id);

            if(room == null)
            {
                return NotFound();
            }

            // vreau doar host sau admin
            if (!isAdmin && room.HostUserId != currentUserId)
                return Forbid();

            ViewBag.Playlists = new SelectList(GetAllPlaylists(), "Value", "Text", room.PlaylistId);

            return View(room);
        }

        [Authorize(Roles ="User,Artist,Admin")]
        [HttpPost]
        public IActionResult Edit(SessionRoom model)
        {
            var currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            var room = db.SessionRooms
                    .FirstOrDefault(sr => sr.Id == model.Id);
            if(room == null)
                return NotFound();
            if(!isAdmin && room.HostUserId != currentUserId)
                return Forbid();

            room.Name = model.Name;
            room.PlaylistId = model.PlaylistId;
            db.SaveChanges();

            TempData["message"] = "Session room updated successfully.";
            TempData["messageType"] = "alert-success";

            return RedirectToAction("Show", new { id = room.Id });
        }

        [Authorize(Roles="User,Artist,Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            var room = db.SessionRooms
                         .Include(sr => sr.Playlist)
                         .FirstOrDefault(sr => sr.Id == id);

            if (room == null)
                return NotFound();

            // doar host sau admin
            if (!isAdmin && room.HostUserId != currentUserId)
                return Forbid();

            return View(room);
        }

        [Authorize(Roles="User,Artist,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            var room = db.SessionRooms
                         .Include(sr => sr.SessionRoomUsers)
                         .FirstOrDefault(sr => sr.Id == id);

            if (room == null)
                return NotFound();

            // doar host sau admin
            if (!isAdmin && room.HostUserId != currentUserId)
                return Forbid();

            // sterg si relatiile M-M
            db.SessionRoomUsers.RemoveRange(room.SessionRoomUsers);
            db.SessionRooms.Remove(room);
            db.SaveChanges();

            TempData["message"] = "Session room deleted successfully.";
            TempData["messageType"] = "alert-success";

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "User,Artist,Admin")]
        [HttpPost]
        public IActionResult AddParticipant(int roomId, string userCode)
        {
            var room = db.SessionRooms
                .Include(r => r.SessionRoomUsers)
                .FirstOrDefault(r => r.Id == roomId);

            if (room == null)
                return NotFound();

            // doar host ul poate ad participanti
            var currentUserId = _userManager.GetUserId(User);
            if (room.HostUserId != currentUserId)
                return Forbid();

            // caut user dupa usercode
            var user = db.Users.FirstOrDefault(u => u.UserCode == userCode);
            if (user == null)
            {
                TempData["message"] = "User with this code does not exist.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Show", new { id = roomId });
            }

            // host ul nu se poate adauga pe el insusi
            if (user.Id == room.HostUserId)
            {
                TempData["message"] = "You are already the host of this session room.";
                TempData["messageType"] = "alert-warning";
                return RedirectToAction("Show", new { id = roomId });
            }

            // verif daca e deja in room
            bool alreadyInRoom = room.SessionRoomUsers
                .Any(sru => sru.UserId == user.Id);

            if (alreadyInRoom)
            {
                TempData["message"] = "User is already in this session room.";
                TempData["messageType"] = "alert-warning";
                return RedirectToAction("Show", new { id = roomId });
            }

            // adaugare
            db.SessionRoomUsers.Add(new SessionRoomUser
            {
                SessionRoomId = roomId,
                UserId = user.Id
            });

            db.SaveChanges();

            TempData["message"] = "User added to session room.";
            TempData["messageType"] = "alert-success";

            return RedirectToAction("Show", new { id = roomId });
        }

        [Authorize(Roles="User,Artist,Admin")]
        [HttpPost]
        public IActionResult RemoveParticipant(int roomId, string userId)
        {
            var currentUserId = _userManager.GetUserId(User);
            bool isAdmin = User.IsInRole("Admin");

            var room = db.SessionRooms
                .Include(sr => sr.SessionRoomUsers)
                .FirstOrDefault(sr => sr.Id == roomId);

            if (room == null)
                return NotFound();

            if (!isAdmin && room.HostUserId != currentUserId)
                return Forbid();

            var membership = room.SessionRoomUsers
                .FirstOrDefault(sru => sru.UserId == userId);

            if (membership == null)
                return NotFound();

            db.SessionRoomUsers.Remove(membership);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = roomId });
        }

        [Authorize(Roles ="User,Artist")]
        [HttpPost]
        public IActionResult Leave(int id)
        {
            var currentUserId = _userManager.GetUserId(User);

            var membership = db.SessionRoomUsers
                .FirstOrDefault(sru => sru.SessionRoomId == id && sru.UserId == currentUserId);

            if (membership == null)
            {
                return Unauthorized();
            }

            db.SessionRoomUsers.Remove(membership);
            db.SaveChanges();

            TempData["message"] = "You left the session room!";
            TempData["messageType"] = "alert-success";

            return RedirectToAction("Index");
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllPlaylists()
        {
            var selectList = new List<SelectListItem>();

            var plys = from ply in db.Playlists
                       where ply.IsPublic == true
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
