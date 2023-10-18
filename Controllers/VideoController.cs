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
        public IActionResult Index(string slug, int id)
        {
            ViewBag.Slug = slug;
            var pr = (from _pr in _context.Video
                     where _pr.Id == id
                     select new VideoVM
                     {
                         VideoLink = _pr.VideoLink,
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

           

            var combinedViewModel = new CombinedViewModel
            {
                VideoVMOj = pr,
            };

            return View(combinedViewModel);
        }
    }
}
