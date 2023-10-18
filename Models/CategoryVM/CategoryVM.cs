using System.ComponentModel.DataAnnotations;
using Tommava.Models.videoVM;

namespace Tommava.Models.CategoryVM
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Display(Name = "Danh mục")]
        public string Name { get; set; }
        public string? Icon { get; set; }
        public IFormFile IconFile { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Slug { get; set; }

        public List<VideoVM> listDataVideo { get; set; }

        public static implicit operator CategoryVM(Category category)
        {
            return new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate,
                IsDeleted = category.IsDeleted,
                Icon = category.Icon,
                Slug = category.Slug,
            };
        }

        public static implicit operator Category(CategoryVM vm)
        {
            return new Category
            {
                Id = vm.Id,
                Name = vm.Name,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
                Icon = vm.Icon??"",
                Slug = vm.Slug,
            };
        }
    }
}
