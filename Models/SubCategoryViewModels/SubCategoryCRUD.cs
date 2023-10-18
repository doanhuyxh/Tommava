using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Tommava.Models.SubCategoryViewModels
{
    public class SubCategoryCRUD
    {
        public int ID { get; set; }
        public int? CategoriesId { get; set; }
        public string? SubName { get; set; }
        public string? Slug { get; set; }
        public string? NameCategory { get; set; }

        public static implicit operator SubCategoryCRUD(SubCategories _PrModel)
        {
            return new SubCategoryCRUD
            {
                ID = _PrModel.ID,
                SubName = _PrModel.SubName,
                CategoriesId = _PrModel.CategoriesId,
                Slug = _PrModel.Slug,
              
            };
        }

        public static implicit operator SubCategories(SubCategoryCRUD vm)
        {
            return new SubCategories
            {
                ID = vm.ID,
                SubName = vm.SubName,
                CategoriesId = vm.CategoriesId,
                Slug = vm.Slug,
            };
        }
    }
}
