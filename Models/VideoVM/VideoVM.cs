using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.videoVM
{
    public class VideoVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên video")]
        public string Name { get; set; }
        [Display(Name = "Chi tiết video")]
        public string Description { get; set; }
        public string? Img { get; set; }
        public string? VideoLink { get; set; }
        [Display(Name = "Thể loại")]
        public int GenreId { get; set; }
        public string? GenreName { get; set; }
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsHome { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Video")]
        public IFormFile? VideoFile { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ImgFile { get; set; }
       

        public static implicit operator VideoVM(Video video)
        {
            return new VideoVM
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                Img = video.Img,
                VideoLink = video.VideoLink,
                GenreId = video.GenreId,
                CategoryId = video.CategoryId,
                IsHome = video.IsHome,
                IsActive = video.IsActive,
                CreatedDate = video.CreatedDate,
                IsDeleted = video.IsDeleted,
            };
        }
        public static implicit operator Video(VideoVM vm)
        {
            return new Video
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                Img = vm.Img ?? "",
                VideoLink = vm.VideoLink ?? "",
                GenreId = vm.GenreId,
                IsHome = vm.IsHome,
                IsActive = vm.IsActive,
                CategoryId = vm.CategoryId,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
