using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;

namespace Orpheo.Controllers
{
    public class TagsController(ApplicationDbContext context,
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
            var tags = db.Tags
                         .Include(t => t.SongTags)
                         .OrderBy(t => t.Name)
                         .ToList();

            ViewBag.Tags = tags;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.AlertType = TempData["messageType"];
            }

            return View();
        }


        [Authorize(Roles = "User,Artist,Admin")]
        public IActionResult Show(int id)
        {
            Tag? tag = db.Tags
                         .Include(t => t.SongTags)
                            .ThenInclude(st => st.Song)
                         .Where(t => t.Id == id)
                         .FirstOrDefault();

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult New()
        {
            Tag tag = new Tag();
            return View(tag);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();

                TempData["message"] = "Tag-ul a fost adăugat!";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }

            return View(tag);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Tag? tag = db.Tags.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Tag requestTag)
        {
            Tag? tag = db.Tags.Find(id);

            if (tag == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                tag.Name = requestTag.Name;

                db.SaveChanges();

                TempData["message"] = "Tag-ul a fost modificat!";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }
            
            return View(requestTag);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Tag? tag = db.Tags
                         .Include(t => t.SongTags)
                         .Where(t => t.Id == id)
                         .FirstOrDefault();

            if (tag == null)
            {
                return NotFound();
            }

            db.SongTags.RemoveRange(tag.SongTags);

            db.Tags.Remove(tag);
            db.SaveChanges();

            TempData["message"] = "Tag-ul a fost șters!";
            TempData["messageType"] = "alert-success";

            return RedirectToAction("Index");
        }
    }
}
