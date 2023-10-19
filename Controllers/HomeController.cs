using Tommava.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tommava.Data;
using Tommava.Models.videoVM;
using Microsoft.EntityFrameworkCore;
using Tommava.Models.CategoryVM;
using Tommava.Models.CombinedViewModel;
using Tommava.Models.Page;

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
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var cate = await _context.Category.OrderByDescending(x=>x.CreatedDate).ToListAsync();
            return Ok(cate);
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20, string filter = "news")
        {
            ViewBag.userName = User.Identity.Name;
            ViewBag.filter = filter;

            var latestCategoryId = await _context.Category
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            IQueryable<VideoVM> queryToFilter = from i in _context.Video
                                                where i.IsDeleted == false && i.CategoryId == latestCategoryId
                                                select new VideoVM
                                                {
                                                    Id = i.Id,
                                                    Name = i.Name,
                                                    Description = i.Description,
                                                    NameVn = i.NameVn,
                                                    Img = i.Img,
                                                    Slug = i.Slug,
                                                    ShortDescription =i.ShortDescription,
                                                    CategoryId = i.CategoryId,
                                                    CategoryName = _context.Category.FirstOrDefault(c => c.Id == i.CategoryId)!.Name ?? "",
                                                    GenreId = i.GenreId,
                                                    GenreName = _context.Genre.FirstOrDefault(c => c.Id == i.GenreId)!.Name ?? "",
                                                    IsDeleted = i.IsDeleted,
                                                    IsActive = i.IsActive,
                                                    IsHome = i.IsHome,
                                                    ViewCount = i.ViewCount,
                                                    CreatedDate = i.CreatedDate,
                                                };

            IQueryable<VideoVM> query = null;

            switch (filter)
            {
                case "views":
                    query = queryToFilter.OrderByDescending(x => x.ViewCount);
                    break;
                case "hot":
                    // Thêm logic lọc theo tiêu chí "Được đề xuất"
                    query = queryToFilter.OrderByDescending(x => x.ViewCount);
                    break;
                case "news":
                default:
                    query = queryToFilter.OrderByDescending(x => x.CreatedDate);
                    break;
            }


            var totalVideos = await query.CountAsync();

            var videos = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categoryName = _context.Category.FirstOrDefault(c => c.Id == latestCategoryId)?.Name;
            ViewBag.Category = categoryName ?? "Default Category Name";
            ViewBag.IdCategory = latestCategoryId;

            var combinedViewModel = new CombinedViewModel
            {
                VideoData = videos,
                VideoPage = new PagedResults<VideoVM>
                {
                    Items = videos,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalVideos
                }
            };

            return View(combinedViewModel);
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