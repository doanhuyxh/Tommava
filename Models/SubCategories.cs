namespace Tommava.Models
{
    public class SubCategories
    {
        public int ID { get; set; }
        public int? CategoriesId { get; set; }
        public string? SubName { get; set; }
        public string? Slug { get; set; }
    }
}
