using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tommava.Data;
using Tommava.Models;
using Tommava.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//cấu hình lại config hệ thống với file applicatin.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMvc();

//Add connetdatabase
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("MSSQL")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<ApplicationUser>>();

builder.Services.AddScoped<IIdentityDataInitializer, IdentityDataInitializer>();

//add service system
builder.Services.AddScoped<ICommon, Common>();
//add send mail
builder.Services.AddScoped<IEmailGoogle, EmailGoogle>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//thêm session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "State1";
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Thời gian không hoạt động trước khi phiên hết hạn
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập thông qua HTTP
    options.Cookie.IsEssential = true; // Đảm bảo phiên vẫn hoạt động ngay cả khi người dùng không đồng ý cookie
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "WebAppTommava"; // Tên cookie
    options.Cookie.Domain = ""; // Tên miền cookie áp dụng (nếu có)
    options.Cookie.Path = "/"; // Đường dẫn cookie áp dụng
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Chính sách bảo mật (SameAsRequest, Always, None)
    options.Cookie.HttpOnly = true; // Cookie chỉ được truy cập bằng HTTP (không bằng JavaScript)
    options.Cookie.SameSite = SameSiteMode.Strict; // Giới hạn cookie chỉ được gửi trong cùng nguồn (Strict, Lax, None)
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn của cookie
    options.SlidingExpiration = true; // Cho phép cập nhật thời gian hết hạn khi có hoạt động từ người dùng
    options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn chuyển hướng khi truy cập bị từ chối
});



var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseRouting();
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
      name: "admin",
      pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}",
      defaults: new { area = "Admin" });

});

app.MapControllers();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var initializer = services.GetRequiredService<IIdentityDataInitializer>();
    await initializer.SeedData(
        services.GetRequiredService<UserManager<ApplicationUser>>(),
        services.GetRequiredService<RoleManager<IdentityRole>>()
    );
}

app.Run();
