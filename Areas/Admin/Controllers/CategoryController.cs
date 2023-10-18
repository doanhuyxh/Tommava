using Tommava.Models.CategoryVM;
using Tommava.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tommava.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tommava.Services;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        public CategoryController(ApplicationDbContext context, ICommon icommon) {
            _context = context;
            _icommon = icommon;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Category.Where(i => i.IsDeleted == false).OrderByDescending(i => i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            CategoryVM Category = new CategoryVM();
            return View(Category);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(CategoryVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Category Category = new Category();
                    if (vm.Id == 0)
                    {
                        Category = vm;
                        Category.CreatedDate = DateTime.Now;
                        if(vm.IconFile != null)
                        {
                            Category.Icon = await _icommon.UploadIconAsync(vm.IconFile);
                        }
                        _context.Add(Category);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = Category;
                        return Ok(json);
                    }
                    else
                    {
                        Category = await _context.Category.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        vm.CreatedDate = Category.CreatedDate;
                        if (vm.IconFile != null)
                        {
                            vm.Icon = await _icommon.UploadIconAsync(vm.IconFile);
                        }
                        _context.Entry(Category).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();
                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = Category;
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
            CategoryVM Category = await _context.Category.FirstOrDefaultAsync(i => i.Id == id);
            return View(Category);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Category Category = await _context.Category.FirstOrDefaultAsync(i => i.Id == id);
            if (Category == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.Category.Remove(Category);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }
    }
}
