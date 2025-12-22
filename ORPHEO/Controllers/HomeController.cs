using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;

namespace Orpheo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var songs = _context.Songs
                .Include(s => s.User)
                .Include(s => s.SongTags)
                    .ThenInclude(st => st.Tag)
                .Select(s => new
                {
                    Song = s,
                    LikesCount = _context.SongVotes
                        .Count(v => v.SongId == s.Id && v.IsLike)
                })
                .OrderByDescending(x => x.LikesCount)
                .Take(6)
                .Select(x => x.Song)
                .ToList();

            return View(songs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
