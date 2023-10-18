﻿using Tommava.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tommava.Models.videoVM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tommava.Models;
using Microsoft.EntityFrameworkCore;
using Tommava.Services;
using Tommava.Models.TimeLineVideoVM;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        public VideoController(ApplicationDbContext context, ICommon common)
        {
            _context = context;
            _icommon = common;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await (from i in _context.Video
                            where i.IsDeleted == false
                            select new VideoVM
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Description = i.Description,
                                Img = i.Img,
                                VideoLink = i.VideoLink,
                                CategoryId = i.CategoryId,
                                CategoryName = _context.Category.FirstOrDefault(c => c.Id == i.CategoryId)!.Name ?? "",
                                GenreId = i.GenreId,
                                GenreName = _context.Genre.FirstOrDefault(c => c.Id == i.GenreId)!.Name ?? "",
                                IsDeleted = i.IsDeleted,
                                IsActive = i.IsActive,
                                IsHome = i.IsHome,
                                CreatedDate = i.CreatedDate,
                            }).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        public IActionResult AddVideo()
        {
            VideoVM video = new VideoVM();
            List<ItemDropDown> itemCate = (from _l in _context.Category
                                           where _l.IsDeleted == false
                                           select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Cate = new SelectList(itemCate, "Id", "Name");

            List<ItemDropDown> itemSubCate = (from _l in _context.SubCategories
                                           where _l.IsDeleted == false
                                           select new ItemDropDown { Id = _l.Id, Name = _l.SubName }).ToList();
            ViewBag.SubCate = new SelectList(itemSubCate, "Id", "Name");

            List<ItemDropDown> itemGenre = (from _l in _context.Genre
                                            where _l.IsDeleted == false
                                            select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Genre = new SelectList(itemGenre, "Id", "Name");

            return View(video);
        }
        public IActionResult EditVideo(int id)
        {
            VideoVM video = _context.Video.FirstOrDefault(i => i.Id == id);

            List<ItemDropDown> itemCate = (from _l in _context.Category
                                           where _l.IsDeleted == false
                                           select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Cate = new SelectList(itemCate, "Id", "Name");

            List<ItemDropDown> itemGenre = (from _l in _context.Genre
                                            where _l.IsDeleted == false
                                            select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Genre = new SelectList(itemGenre, "Id", "Name");

            return View(video);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditVideo(VideoVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Video video = new Video();
                    if (vm.Id == 0)
                    {
                        if (vm.ImgFile != null)
                        {
                            vm.Img = await _icommon.UploadImgVideoAsync(vm.ImgFile);
                        }
                        if (vm.VideoFile != null)
                        {
                            vm.VideoLink = await _icommon.UploadVideoAsync(vm.VideoFile);
                        }
                        video = vm;
                        video.CreatedDate = DateTime.Now;
                        video.IsHome = true;
                        video.IsDeleted = false;
                        _context.Add(video);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = video;
                        return Ok(json);
                    }
                    else
                    {
                        video = await _context.Video.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        vm.CreatedDate = video.CreatedDate;
                        if (vm.ImgFile != null)
                        {
                            vm.Img = await _icommon.UploadImgVideoAsync(vm.ImgFile);
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
                else
                {
                    json.Message = ModelState.ValidationState.ToString();
                    json.StatusCode = 500;
                    json.Object = null;
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
        public async Task<IActionResult> DeleteVideo(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Models.Video video = await _context.Video.FirstOrDefaultAsync(i => i.Id == id);
            if (video == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.Video.Remove(video);
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
                for(int i = 0; i < vm.Count; i++)
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
    }
}

