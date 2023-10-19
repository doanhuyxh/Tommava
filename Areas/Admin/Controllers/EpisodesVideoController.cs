using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tommava.Data;
using Tommava.Models.videoVM;
using Tommava.Models;
using Tommava.Services;
using Tommava.Models.EpisodesVideoVM;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tommava.Models.TimeLineVideoVM;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EpisodesVideoController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        public EpisodesVideoController(ApplicationDbContext context, ICommon common)
        {
            _context = context;
            _icommon = common;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await (from i in _context.EpisodesVideo
                            where i.IsDelete == false
                            select new EpisodesVideoVM
                            {
                                Id = i.Id,
                                ViewCount = i.ViewCount,
                                VideoLink = i.VideoLink,
                                VideoId = i.VideoId,
                                TitleVN = i.TitleVN,
                                TitleEN = i.TitleEN,
                                IsDelete = i.IsDelete,
                                Description = i.Description,
                                EpisodeNumber = i.EpisodeNumber,
                                ImgLink = i.ImgLink,
                                VideoName = _context.Video.FirstOrDefault(v=>v.Id==i.VideoId)!.Name,
                            }).OrderByDescending(d=>d.VideoId).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {

            List<ItemDropDown> itemVideo = (from _l in _context.Video
                                            where _l.IsDeleted == false
                                            select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Video = new SelectList(itemVideo, "Id", "Name");

            EpisodesVideoVM vm = new EpisodesVideoVM();
            return PartialView("_AddEditData", vm);
        }
        [HttpGet]
        public IActionResult EditData(int id)
        {

            List<ItemDropDown> itemVideo = (from _l in _context.Video
                                            where _l.IsDeleted == false
                                            select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Video = new SelectList(itemVideo, "Id", "Name");

            EpisodesVideoVM vm = _context.EpisodesVideo.FirstOrDefault(i => i.Id == id);
            return PartialView("_AddEditData", vm);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [AllowAnonymous]
        public async Task<IActionResult> SaveData(EpisodesVideoVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                EpisodesVideo video = new EpisodesVideo();
                if (vm.Id == 0)
                {
                    if (vm.ImgFile != null)
                    {
                        vm.ImgLink = await _icommon.UploadImgVideoAsync(vm.ImgFile);
                    }
                    if (vm.VideoFile != null)
                    {
                        vm.VideoLink = await _icommon.UploadVideoAsync(vm.VideoFile);
                    }

                    video = vm;
                    video.IsDelete = false;
                    _context.Add(video);
                    await _context.SaveChangesAsync();

                    json.Message = "Success";
                    json.StatusCode = 200;
                    json.Object = video;
                    return Ok(json);
                }
                else
                {
                    video = await _context.EpisodesVideo.FirstOrDefaultAsync(i => i.Id == vm.Id);                    
                    if (vm.ImgFile != null)
                    {
                        vm.ImgLink = await _icommon.UploadImgVideoAsync(vm.ImgFile);
                    }
                    if (vm.VideoFile != null)
                    {
                        vm.VideoLink = await _icommon.UploadVideoAsync(vm.VideoFile);
                    }
                    _context.Entry(video).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();
                    json.Message = "Success";
                    json.StatusCode = 200;
                    json.Object = video;
                    return Ok(json);
                }


            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                json.Object = ex;
                return Ok(json);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteData(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Models.EpisodesVideo video = await _context.EpisodesVideo.FirstOrDefaultAsync(i => i.Id == id);
            if (video == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.EpisodesVideo.Remove(video);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }

        [HttpGet]
        public async Task<IActionResult> TimeLine(int videoid)
        {
            List<TimeLineVideo> timeLineVideos = await _context.TimeLineVideo.Where(i => i.VideoId == videoid).OrderBy(i => i.TimePoint).ToListAsync();
            return PartialView("_TimeLine", timeLineVideos);

        }

        [HttpGet]
        public IActionResult AddTimeLine()
        {
            TimeLineVideoVM vm = new TimeLineVideoVM();
            return PartialView("_AddEditTimeLine");
        }
        [HttpGet]
        public async Task<IActionResult> EditTimeLine(int id)
        {
            TimeLineVideoVM vm = await _context.TimeLineVideo.FirstOrDefaultAsync(i => i.Id == id);
            return PartialView("_AddEditTimeLine", vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTimeLine(TimeLineVideoVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                TimeLineVideo timeLine = new TimeLineVideo();
                if (vm.Id == 0)
                {
                    timeLine = vm;
                    timeLine.CreatedDate = DateTime.Now;
                    timeLine.IsDeleted = false;
                    _context.Add(timeLine);
                    await _context.SaveChangesAsync();

                    json.Object = timeLine;
                    json.Message = "";
                    json.StatusCode = 201;
                    return Json(json);
                }
                else
                {
                    timeLine = await _context.TimeLineVideo.FirstOrDefaultAsync(i => i.Id == vm.Id);
                    vm.CreatedDate = timeLine.CreatedDate;
                    _context.Entry(timeLine).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();


                    json.Object = timeLine;
                    json.Message = "";
                    json.StatusCode = 202;
                    return Json(json);
                }
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.Object = vm;
                json.StatusCode = 500;
                return Ok(json);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTimeLine(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Models.TimeLineVideo timeLineVideo = await _context.TimeLineVideo.FirstOrDefaultAsync(i => i.Id == id);
            if (timeLineVideo == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.TimeLineVideo.Remove(timeLineVideo);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }

        [HttpGet]
        public IActionResult AddTimeLineExcel()
        {
            return PartialView("_AddTimeLineExcel");
        }

        [HttpPost]
        public async Task<IActionResult> SaveTimeLineRange(List<TimeLineVideoVM> vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                for (int i = 0; i < vm.Count; i++)
                {
                    TimeLineVideo t = new TimeLineVideo();
                    t.VideoId = vm[i].VideoId;
                    t.Vietnamese = vm[i].Vietnamese;
                    t.English = vm[i].English;
                    t.TimePoint = vm[i].TimePoint;
                    t.CreatedDate = DateTime.Now;
                    t.IsDeleted = false;

                    await _context.AddAsync(t);
                    await _context.SaveChangesAsync();
                }

                json.Message = "Success";
                json.StatusCode = 201;
                json.Object = null;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.StatusCode = 500;
                json.Message = ex.Message;
                json.Object = vm;
                return BadRequest(json);
            }
        }
        private const string UploadDirectory = "wwwroot/upload/chunkVideo";
        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public IActionResult UploadVideoChunk([FromForm] IFormFile chunk, int totalChunks, int chunkIndex, string fileName)
        {
            try
            {
                // Đảm bảo thư mục lưu trữ phần nhỏ đã được tạo
                string uploadPath = Path.Combine(UploadDirectory, fileName);
                string chunkPath = Path.Combine(uploadPath, chunkIndex.ToString());

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Lưu trữ phần nhỏ của tệp video
                using (var stream = new FileStream(chunkPath, FileMode.Create))
                {
                    chunk.CopyTo(stream);
                }

                // Kiểm tra xem đã nhận đủ số lượng phần nhỏ để ghép tệp chưa
                if (IsAllChunksReceived(uploadPath, totalChunks))
                {
                    MergeChunks(uploadPath, fileName, totalChunks);

                    // Trả về đường dẫn của tệp video gốc sau khi ghép thành công
                    string finalFilePath = Path.Combine(UploadDirectory, fileName);
                    return Ok(new { FilePath = finalFilePath });
                }

                return Ok("Phần nhỏ đã được nhận.");
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi tải lên: " + ex.Message);
            }
        }

        private bool IsAllChunksReceived(string uploadPath, int totalChunks)
        {
            string[] chunkFiles = Directory.GetFiles(uploadPath);
            return chunkFiles.Length == totalChunks;
        }

        private void MergeChunks(string uploadPath, string fileName, int totalChunks)
        {
            string finalFilePath = Path.Combine(UploadDirectory, fileName);
            using (var finalStream = new FileStream(finalFilePath, FileMode.Create))
            {
                for (int i = 0; i < totalChunks; i++)
                {
                    string chunkPath = Path.Combine(uploadPath, i.ToString());
                    using (var chunkStream = new FileStream(chunkPath, FileMode.Open))
                    {
                        chunkStream.CopyTo(finalStream);
                    }
                }
            }

            // Xóa các phần nhỏ sau khi đã ghép tệp gốc thành công
            Directory.Delete(uploadPath, true);
        }
    }

}

