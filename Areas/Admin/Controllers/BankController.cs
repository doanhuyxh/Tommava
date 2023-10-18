using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tommava.Data;
using Tommava.Models.BankVM;
using Tommava.Models;
using Tommava.Services;
using Microsoft.AspNetCore.Authorization;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BankController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _common;

        public BankController(ApplicationDbContext context, ICommon common)
        {
            _context = context;
            _common = common;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Bank.Where(i => i.IsDeleted == false).OrderByDescending(i => i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            BankVM bank = new BankVM();
            return View(bank);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(BankVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Bank bank = new Bank();
                    if (vm.Id == 0)
                    {
                        bank = vm;
                        bank.CreatedDate = DateTime.Now;
                        bank.IsDeleted = false;
                        _context.Add(bank);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = bank;
                        return Ok(json);
                    }
                    else
                    {
                        bank = await _context.Bank.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        vm.CreatedDate = bank.CreatedDate;
                        _context.Entry(bank).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = bank;
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
            BankVM bank = await _context.Bank.FirstOrDefaultAsync(i => i.Id == id);
            return View(bank);
        }

        [HttpDelete]
        public async Task<IActionResult> Deletebank(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Models.Bank bank = await _context.Bank.FirstOrDefaultAsync(i => i.Id == id);
            if (bank == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.Bank.Remove(bank);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }

        public async Task<IActionResult> ChangeActive(int id)
        {
            Bank bank = await _context.Bank.FirstOrDefaultAsync(i=>i.Id == id);
            if (bank != null)
            {
                bank.IsActive =  !bank.IsActive;
                _context.Update(bank);
                await _context.SaveChangesAsync();
                return Ok(bank);
            }
            else
            {
                return Ok(bank);
            }

        }
    }
}
