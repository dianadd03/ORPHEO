using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Orpheo.Data;
using Orpheo.Models;

namespace Orpheo.Controllers
{
    public class CommsController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly ApplicationDbContext db = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;



        [HttpPost]
        [Authorize(Roles = "User,Artist,Admin")]
        public IActionResult New(Comm comm)
        {
            comm.CreatedAt = DateTime.Now;
            comm.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Comms.Add(comm);
                db.SaveChanges();
                return Redirect("/Songs/Show/" + comm.SongId);
            }
            else
            {
                return Redirect("/Songs/Show/" + comm.SongId);
            }
        }


        [HttpPost]
        [Authorize(Roles = "User,Artist,Admin")]
        public IActionResult Delete(int id)
        {
            Comm? comm = db.Comms.Find(id);

            if (comm is null)
            {
                return NotFound();
            }
            else
            {
                // Doar autorul sau adminul
                if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    db.Comms.Remove(comm);
                    db.SaveChanges();
                    return Redirect("/Songs/Show/" + comm.SongId);
                }
                else
                {
                    TempData["message"] = "Nu aveți dreptul să ștergeți acest comentariu!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Songs");
                }
            }
        }


        [Authorize(Roles = "User,Artist,Admin")]
        public IActionResult Edit(int id)
        {
            Comm? comm = db.Comms.Find(id);

            if (comm == null)
            {
                return NotFound();
            }
            else
            {
                if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    return View(comm);
                }
                else
                {
                    TempData["message"] = "Nu aveți dreptul să editați acest comentariu!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Songs");
                }
            }
        }



        [HttpPost]
        [Authorize(Roles = "User,Artist,Admin")]
        public IActionResult Edit(int id, Comm requestComm)
        {
            Comm? comm = db.Comms.Find(id);

            if (comm == null)
            {
                return NotFound();
            }
            else
            {
                if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    if (ModelState.IsValid)
                    {
                        comm.Text = requestComm.Text;
                        db.SaveChanges();

                        return Redirect("/Songs/Show/" + comm.SongId);
                    }
                    else
                    {
                        return View(requestComm);
                    }
                }
                else
                {
                    TempData["message"] = "Nu aveți dreptul să editați acest comentariu!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Songs");
                }
            }
        }
    }
}
