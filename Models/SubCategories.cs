using System.ComponentModel.DataAnnotations;

namespace Tommava.Models
{
    public class SubCategories
    {
        [Key]
        public int Id { get; set; }
        public int? CategoriesId { get; set; }
        public string? SubName { get; set; }
        public string? Slug { get; set; }
        public bool IsDeleted { get; set; }
    }
}
