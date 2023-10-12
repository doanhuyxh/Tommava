using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Tommava.Data;
using Tommava.Models;
using Tommava.Models.AccountViewModels;

namespace Tommava.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Customer()
        {
            return View();
        }
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await (from i in _context.Users
                            where i.UserName != "supperadmin"
                            select new UserProFileViewModel
                            {
                                ApplicationUserId = i.Id,
                                AvatarPath = i.Avatar,
                                UserName = i.UserName,
                                Email = i.Email,
                                IsActive = i.IsActive,
                                About = i.About,
                                FullName = i.FullName,
                                DateVipCreate = i.DateVipCreate,
                                VipId = i.VipId,
                                CreateDate = i.CreateDate,
                                VipName = _context.VipPackage.FirstOrDefault(v => v.Id == i.VipId).Name,
                            }).ToListAsync();

            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        public async Task<IActionResult> ChangeActive(string useName)
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(i => i.UserName == useName);
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                user.IsActive = !user.IsActive;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVip(int vipId, string useName)
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(i => i.UserName == useName);
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                user.VipId = vipId;
                user.DateVipCreate = DateTime.Now;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
        }
    }
}
