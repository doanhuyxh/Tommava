using Tommava.Data;
using Tommava.Models;

namespace Tommava.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context, IConfiguration configuration)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> UploadIconAsync(IFormFile file)
        {
            string path = string.Empty;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/icon");

                if (file.FileName == null)
                    path = "logo.png";
                else
                    path = DateTime.Now.Ticks.ToString() + ".png";
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/icon/{path}";
        }
        
        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            string path = string.Empty;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/Video");

                if (file.FileName == null)
                    path = "";
                else
                    path = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/Video/{path}";
        }
        public async Task<string> UploadImgVideoAsync(IFormFile file)
        {
            string path = string.Empty;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/ImgVideo");

                if (file.FileName == null)
                    path = "video.png";
                else
                    path = DateTime.Now.Ticks.ToString() + ".png";
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/ImgVideo/{path}";
        }


        public IEnumerable<Category> GetAllCategory()
        {
            return _context.Category;
        }

    }
}

