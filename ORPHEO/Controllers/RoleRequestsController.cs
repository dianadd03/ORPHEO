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

        public RoleRequestsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "User")]
        public IActionResult RequestArtistRole()
        {
            var userId = _userManager.GetUserId(User);

            var existingRequest = db.RoleRequests
                .FirstOrDefault(r => r.UserId == userId);

            if (existingRequest != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var request = new RoleRequest
            {
                UserId = userId,
                Status = "Pending"
            };

            db.RoleRequests.Add(request);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PendingRequests()
        {
            var requests = db.RoleRequests
                .Include(r => r.User)
                .Where(r => r.Status == "Pending")
                .ToList();

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var request = db.RoleRequests
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            // scoatem rolul User daca exista
            if (await _userManager.IsInRoleAsync(request.User, "User"))
            {
                await _userManager.RemoveFromRoleAsync(request.User, "User");
            }

            // adaugam rolul Artist
            await _userManager.AddToRoleAsync(request.User, "Artist");

            request.Status = "Approved";
            db.SaveChanges();

            return RedirectToAction("PendingRequests");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            var request = db.RoleRequests
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Rejected";
            db.SaveChanges();

            return RedirectToAction("PendingRequests");
        }
    }
}
