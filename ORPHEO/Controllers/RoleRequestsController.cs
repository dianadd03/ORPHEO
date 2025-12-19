using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;

namespace ORPHEO.Controllers
{
    public class RoleRequestsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        // aici am request ul facut de user pentru a fi artist
        [Authorize(Roles = "User")]
        public IActionResult RequestArtistRole()
        {
            var userId = _userManager.GetUserId(User);

            var existingRequest = db.RoleRequests.FirstOrDefault(r => r.UserId == userId);

            if (existingRequest != null)
            {
                TempData["message"] = "You already have a pending request!";
                TempData["msgType"] = "warning";
                return RedirectToAction("Index", "Home");
            }

            db.RoleRequests.Add(new RoleRequest { UserId = userId });
            db.SaveChanges();

            TempData["message"] = "Your request has been sent!";
            TempData["msgType"] = "success";

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PendingRequests()
        {
            var requests = db.RoleRequests.Include(r => r.User).ToList();
            return View(requests);
        }

        // ADMIN approve
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var request = db.RoleRequests.Include(r => r.User).FirstOrDefault(r => r.Id == id);
            if (request == null) return NotFound();

            // 1. scolt rolul User
            if (await _userManager.IsInRoleAsync(request.User, "User"))
            {
                await _userManager.RemoveFromRoleAsync(request.User, "User");
            }

            // 2. adaug rolul Artist
            await _userManager.AddToRoleAsync(request.User, "Artist");

            db.RoleRequests.Remove(request);
            db.SaveChanges();

            TempData["message"] = "Felicitari! Sunteti artist!";
            TempData["msgType"] = "success";
            return RedirectToAction("PendingRequests");
        }

        // ADMIN reject
        [Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            var request = db.RoleRequests.FirstOrDefault(r => r.Id == id);
            if (request == null) return NotFound();

            db.RoleRequests.Remove(request);
            db.SaveChanges();

            TempData["message"] = "Ne pare rau! Nu sunteti un artist!";
            TempData["msgType"] = "danger";
            return RedirectToAction("PendingRequests");
        }

    }
}
