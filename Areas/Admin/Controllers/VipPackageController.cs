using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tommava.Data;
using Tommava.Models.VipPackageVM;
using Tommava.Models;
using Tommava.Services;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VipPackageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _common;

        public VipPackageController(ApplicationDbContext context, ICommon common)
        {
            _context = context;
            _common = common;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.VipPackage.Where(i => i.IsDeleted == false).OrderByDescending(i => i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            VipPackageVM VipPackage = new VipPackageVM();
            return View(VipPackage);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(VipPackage vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    VipPackage VipPackage = new VipPackage();
                    if (vm.Id == 0)
                    {
                        VipPackage = vm;
                        VipPackage.CreatedDate = DateTime.Now;
                        VipPackage.IsDeleted = false;
                        _context.Add(VipPackage);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = VipPackage;
                        return Ok(json);
                    }
                    else
                    {
                        VipPackage = await _context.VipPackage.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        vm.CreatedDate = VipPackage.CreatedDate;
                        _context.Entry(VipPackage).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = VipPackage;
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
            VipPackageVM VipPackage = await _context.VipPackage.FirstOrDefaultAsync(i => i.Id == id);
            return View(VipPackage);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVipPackage(int id)
        {
            JsonResultVM json = new JsonResultVM();
            VipPackage VipPackage = await _context.VipPackage.FirstOrDefaultAsync(i => i.Id == id);
            if (VipPackage == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.VipPackage.Remove(VipPackage);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }
    }
}
