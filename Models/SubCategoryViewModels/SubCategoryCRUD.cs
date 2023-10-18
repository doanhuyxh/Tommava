using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Tommava.Models.SubCategoryViewModels
{
    public class SubCategoryCRUD
    {
        public int Id { get; set; }
        [Display(Name = "Danh mục")]
        public int? CategoriesId { get; set; }
        [Display(Name = "Tên danh mục phụ")]
        public string? SubName { get; set; }
        [Display(Name = "Slug")]
        public string? Slug { get; set; }
        public string? NameCategory { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator SubCategoryCRUD(SubCategories _PrModel)
        {
            return new SubCategoryCRUD
            {
                Id = _PrModel.Id,
                SubName = _PrModel.SubName,
                CategoriesId = _PrModel.CategoriesId,
                Slug = _PrModel.Slug,
                IsDeleted = _PrModel.IsDeleted,
            };
        }

        public static implicit operator SubCategories(SubCategoryCRUD vm)
        {
            return new SubCategories
            {
                Id = vm.Id,
                SubName = vm.SubName,
                CategoriesId = vm.CategoriesId,
                Slug = vm.Slug,
                IsDeleted= vm.IsDeleted,
            };
        }
    }
}
