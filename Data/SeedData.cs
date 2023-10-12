using Microsoft.AspNetCore.Identity;
using Tommava.Models;

namespace Tommava.Data
{
    public interface IIdentityDataInitializer
    {
        Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager);
    }

    public class IdentityDataInitializer : IIdentityDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityDataInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            // Add roles          
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Member"));

            // Add super admin user
            var superAdminEmail = _configuration["SuperAdminDefaultOption:Email"];
            var superAdminUserName = _configuration["SuperAdminDefaultOption:Username"];
            var superAdminPassword = _configuration["SuperAdminDefaultOption:Password"];
            var superAdminUser = new ApplicationUser
            {
                Email = superAdminEmail,
                UserName = superAdminUserName,
                FullName = "Nguyễn văn A",
                IsActive = true,
                Avatar = "/upload/avatar/blank_avatar.png",
                CreateDate = DateTime.Now,
            };
           
            var memberUser = new ApplicationUser
            {
                Email = "memberUser@gmail.com",
                UserName = "memberUser",
                FullName = "Khách Hàng 1",
                IsActive = true,
                Avatar = "/upload/avatar/blank_avatar.png",
                CreateDate = DateTime.Now,
    };

            var result1 = await userManager.CreateAsync(superAdminUser, superAdminPassword);
            var result3 = await userManager.CreateAsync(memberUser, superAdminPassword);


            if (result1.Succeeded && result3.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, "Admin");
                await userManager.AddToRoleAsync(memberUser, "Member");
            }
        }
    }

}
