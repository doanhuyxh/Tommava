using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.videoVM
{
    public class VideoVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên tiếng anh")]
        public string Name { get; set; }
        [Display(Name = "Chi tiết video")]
        public string Description { get; set; }
        [Display(Name="Mô tả ngắn")]
        public string ShortDescription { get; set; }
        public string? Img { get; set; }
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
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ImgFile { get; set; }

        public List<VideoVM>? listDataVideo { get; set; }

        public List<EpisodesVideoVM.EpisodesVideoVM>? listDataMain { get; set; }


        public EpisodesVideoVM.EpisodesVideoVM? VideoMain { get; set; }

        public List<Category>? listCate { get; set; }

        public List<TimeLineVideoVM.TimeLineVideoVM>? listTimeLine { get; set; }


        public int? ViewCount { get; set; } = 0;
        [Display(Name = "Danh mục phụ")]
        public int? SubCategoryId { get; set; } = 0;
        [Display(Name ="Tên tiếng việt")]
        public string NameVn { get; set; }
        [Display(Name = "Slug")]
        public string Slug { get; set; }

        public static implicit operator VideoVM(Video video)
        {
            return new VideoVM
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                Img = video.Img,
                GenreId = video.GenreId,
                CategoryId = video.CategoryId,
                IsHome = video.IsHome,
                IsActive = video.IsActive,
                CreatedDate = video.CreatedDate,
                IsDeleted = video.IsDeleted,
                ViewCount = video.ViewCount,
                SubCategoryId = video.SubCategoryId,   
                NameVn = video.NameVn,
                Slug = video.Slug,

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
                GenreId = vm.GenreId,
                IsHome = vm.IsHome,
                IsActive = vm.IsActive,
                CategoryId = vm.CategoryId,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
                ViewCount = vm.ViewCount,
                SubCategoryId = vm.SubCategoryId,
                NameVn = vm.NameVn,
                Slug = vm.Slug,
            }; 
        }
    }
}
