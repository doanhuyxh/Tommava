using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Tommava.Data;
using Tommava.Models.CombinedViewModel;
using Tommava.Models.EpisodesVideoVM;
using Tommava.Models.Page;
using Tommava.Models.TimeLineVideoVM;
using Tommava.Models.videoVM;

namespace Tommava.Controllers
{
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VideoController> _logger;

        public VideoController(ILogger<VideoController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;


        }
        [AllowAnonymous]
        [HttpGet]
        [Route("tudien")]

        public IActionResult GetOutputJson()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "output.json");

            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                return Content(json, "application/json");
            }

            return NotFound();
        }
        [Route("video/{slug}")]
        public async Task <IActionResult> Index(string slug, int id, int episode = 1)
        {
            ViewBag.userName = User.Identity.Name;
            ViewBag.Slug = slug;

            var pr = (from _pr in _context.Video
                       where _pr.Id == id
                     select new VideoVM
                     {
                         Id = id,
                         CategoryId = _pr.CategoryId,
                         ShortDescription = _pr.ShortDescription,
                         Name = _pr.Name,
                         NameVn = _pr.NameVn,
                         ViewCount = _pr.ViewCount,
                         Description = _pr.Description,
                         
                           VideoMain = _context.EpisodesVideo
                                        .Where(img => img.VideoId == id && img.EpisodeNumber == episode)
                                        .Select(img => new EpisodesVideoVM
                                        {
                                            Id = img.Id,
                                            VideoLink = img.VideoLink,
                                            EpisodeNumber = img.EpisodeNumber,
                                            TitleEN = img.TitleEN,
                                            TitleVN = img.TitleVN,
                                            Description = img.Description,
                                            ViewCount = img.ViewCount,
                                            ImgLink = img.ImgLink,
                                        }).FirstOrDefault(),
                        
                         listDataMain = _context.EpisodesVideo
                                        .Where(img => img.VideoId == id)
                                        .Select(img => new EpisodesVideoVM
                                        {
                                            Id = img.Id,
                                            EpisodeNumber = img.EpisodeNumber,
                                            TitleEN = img.TitleEN,
                                            TitleVN = img.TitleVN,
                                            Description = img.Description,
                                            ViewCount = img.ViewCount,
                                            ImgLink = img.ImgLink,
                                        }).ToList()

                     }).FirstOrDefault();

            int selectedEpisodeId = pr.VideoMain.Id;
            var timeLineForSelectedEpisode = _context.TimeLineVideo
                                             .Where(img => img.VideoId == selectedEpisodeId)
                                             .Select(img => new TimeLineVideoVM
                                             {
                                                 TimePoint = img.TimePoint,
                                                 VideoId = img.VideoId,
                                                 Vietnamese = img.Vietnamese,
                                                 English = img.English,
                                             }).ToList();
            pr.listTimeLine = timeLineForSelectedEpisode;
            if (pr != null)
            {
                var prRs = _context.Video.Where(img => img.Id == pr.Id).FirstOrDefault();
                prRs.ViewCount++;
                await  _context.SaveChangesAsync();
                var lastTime = (from _last in _context.Video
                                where _last.CategoryId == pr.CategoryId
                                select new VideoVM
                                {
                                    Id = _last.Id,
                                    CategoryId = _last.CategoryId,
                                    ShortDescription = _last.ShortDescription,
                                    Slug = _last.Slug,
                                    Name = _last.Name,
                                    NameVn = _last.NameVn,
                                    ViewCount = _last.ViewCount,
                                    Img = _last.Img,
                                }).ToList();
                var combinedViewModel = new CombinedViewModel
                {
                    VideoVMOj = pr,
                    VideoData = lastTime
                };

                return View(combinedViewModel);
            }

            else
            {
               
                return NotFound();
            }

        }


        [Route("danh-muc/{slug}")]
        public async Task<IActionResult> Category(string slug,int page = 1, int pageSize = 20, string filter = "news")
        {
            ViewBag.Slug = slug;
            ViewBag.filter = filter;

            var latestCategoryId = await _context.Category
                .Where(c => c.Slug == slug)
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
                                                    ShortDescription = i.ShortDescription,
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
       
    }
}
