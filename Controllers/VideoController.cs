using Microsoft.AspNetCore.Mvc;
using Tommava.Data;
using Tommava.Models.CombinedViewModel;
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

        [Route("video/{slug}")]
        public async Task <IActionResult> Index(string slug, int id)
        {
            ViewBag.userName = User.Identity.Name;
            ViewBag.Slug = slug;
            var pr = (from _pr in _context.Video
                     where _pr.Id == id
                     select new VideoVM
                     {
                         Id = id,
                         CategoryId = _pr.CategoryId,
                         VideoLink = _pr.VideoLink,
                         Name = _pr.Name,
                         NameVn = _pr.NameVn,
                         ViewCount = _pr.ViewCount,
                         Description = _pr.Description,
                         listTimeLine = _context.TimeLineVideo
                                        .Where(img => img.VideoId == id)
                                        .Select(img => new TimeLineVideoVM
                                        {
                                            TimePoint = img.TimePoint,
                                            VideoId = img.VideoId,
                                            Vietnamese = img.Vietnamese,
                                            English = img.English,
                                        }).ToList()

                     }).FirstOrDefault();
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
                                    VideoLink = _last.VideoLink,
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
    }
}
