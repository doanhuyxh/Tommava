using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tommava.Data;
using Tommava.Models;
using Tommava.Models.SubCategoryViewModels;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly ApplicationDbContext _context;
        public SubCategoryController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await (from sub in _context.SubCategories 
                            where sub.IsDeleted == false
                            select new SubCategoryCRUD
                            {
                                Slug = sub.Slug,
                                SubName = sub.SubName,
                                NameCategory = _context.Category.FirstOrDefault(i=>i.Id == sub.CategoriesId)!.Name,
                                CategoriesId = sub.CategoriesId,
                                Id= sub.Id,
                                IsDeleted = false,

                            }).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            List<ItemDropDown> itemCate = (from _l in _context.Category
                                           where _l.IsDeleted == false
                                           select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Cate = new SelectList(itemCate, "Id", "Name");
            return PartialView("AddData");
        }
        [HttpGet]
        public IActionResult EditData(int id)
        {
            List<ItemDropDown> itemCate = (from _l in _context.Category
                                           where _l.IsDeleted == false
                                           select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Cate = new SelectList(itemCate, "Id", "Name");

            SubCategoryCRUD vm = _context.SubCategories.FirstOrDefault(c => c.Id == id);

            return PartialView("EditData", vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveData(SubCategoryCRUD vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                
                    SubCategories subcate = new SubCategories();
                    if (vm.Id == 0)
                    {
                        subcate = vm;
                        _context.Add(subcate);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = subcate;
                        return Ok(json);
                    }
                    else
                    {
                        subcate = await _context.SubCategories.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        _context.Entry(subcate).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = subcate;
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
        public IActionResult DeleteSubCategory(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Models.SubCategories subcate =  _context.SubCategories.FirstOrDefault(i => i.Id == id);
            if (subcate == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.SubCategories.Remove(subcate);
                _context.SaveChanges();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }

        [HttpGet]
        public IActionResult GetSubCategoryByCategoryId(int id)
        {
            JsonResultVM json = new JsonResultVM();
            var rs = (from sub in _context.SubCategories
                           where sub.IsDeleted == false && sub.CategoriesId == id
                           select new SubCategoryCRUD
                           {
                               Slug = sub.Slug,
                               SubName = sub.SubName,
                               NameCategory = _context.Category.FirstOrDefault(i => i.Id == sub.CategoriesId)!.Name,
                               CategoriesId = sub.CategoriesId,
                               Id = sub.Id,
                               IsDeleted = false,

                           }).ToList();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }
    }
}
