using Tommava.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tommava.Data;
using Tommava.Models.videoVM;
using Microsoft.EntityFrameworkCore;
using Tommava.Models.CategoryVM;
using Tommava.Models.CombinedViewModel;

namespace Tommava.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;


        }

        public async Task<IActionResult> Index()
        {
            ViewBag.userName = User.Identity.Name;
            var rs = await (from i in _context.Video
                            where i.IsDeleted == false
                            select new VideoVM
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Description = i.Description,
                                Img = i.Img,
                                Slug = i.Slug,
                                VideoLink = i.VideoLink,
                                CategoryId = i.CategoryId,
                                CategoryName = _context.Category.FirstOrDefault(c => c.Id == i.CategoryId)!.Name ?? "",
                                GenreId = i.GenreId,
                                GenreName = _context.Genre.FirstOrDefault(c => c.Id == i.GenreId)!.Name ?? "",
                                IsDeleted = i.IsDeleted,
                                IsActive = i.IsActive,
                                IsHome = i.IsHome,
                                CreatedDate = i.CreatedDate,
                            }).Take(20).OrderByDescending(x => x.CreatedDate).ToListAsync();

            var groupedResults = await (from i in _context.Video
                                        where i.IsDeleted == false
                                        group i by _context.Category.FirstOrDefault(c => c.Id == i.CategoryId)!.Name into categoryGroup
                                        select new CategoryVM 
                                        {
                                            Name = categoryGroup.Key,
                                            listDataVideo = categoryGroup.Select(item => new VideoVM
                                            {
                                                Id = item.Id,
                                                Name = item.Name,
                                                Description = item.Description,
                                                Img = item.Img,
                                                VideoLink = item.VideoLink,
                                                CategoryId = item.CategoryId,
                                                CategoryName = _context.Category.FirstOrDefault(c => c.Id == item.CategoryId)!.Name ?? "",
                                                GenreId = item.GenreId,
                                                GenreName = _context.Genre.FirstOrDefault(c => c.Id == item.GenreId)!.Name ?? "",
                                                IsDeleted = item.IsDeleted,
                                                IsActive = item.IsActive,
                                                IsHome = item.IsHome,
                                                CreatedDate = item.CreatedDate,
                                            }).Take(20).OrderByDescending(x => x.CreatedDate).ToList()
                                        }).ToListAsync();
            var combinedViewModel = new CombinedViewModel
            {
                VideoData = rs,
                CategoryData = groupedResults
            };

            return View(combinedViewModel);
        }
        public async Task<IActionResult> GetNews()
        {
            var categories = _context.Category.ToList();
            var combinedViewModel = new CombinedViewModel
            {
                CategoryData = categories.Select(category => new CategoryVM
                {
                   Name = category.Name,
                   Id  = category.Id,

                }).ToList(),
            };
            return PartialView("_nav", combinedViewModel.CategoryData);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}