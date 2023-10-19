using Tommava.Data;
using Tommava.Models;
using Tommava.Models.GenreVM;
using Tommava.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        public GenreController(ApplicationDbContext context, ICommon common)
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
            var rs = await _context.Genre.Where(i => i.IsDeleted == false).OrderByDescending(i=>i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            GenreVM trademark = new GenreVM();
            return View(trademark);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(GenreVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Models.Genre trademark = new Models.Genre();
                    if (vm.Id == 0)
                    {
                        trademark = vm;
                        trademark.CreatedDate = DateTime.Now;
                        
                        _context.Add(trademark);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = trademark;
                        return Ok(json);
                    }
                    else
                    {
                        trademark = await _context.Genre.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        vm.CreatedDate = trademark.CreatedDate;
                        _context.Entry(trademark).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = trademark;
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
        [HttpGet]
        public async Task<IActionResult> EditData(int id)
        {
            GenreVM trademark = await _context.Genre.FirstOrDefaultAsync(i=>i.Id == id);
            return View(trademark);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTrademark(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Models.Genre trademark = await _context.Genre.FirstOrDefaultAsync(i => i.Id == id);
            if (trademark == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.Genre.Remove(trademark);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }
    }
}
