using System.ComponentModel.DataAnnotations;

namespace Tommava.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; } = string.Empty;
        public string VideoLink { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsHome { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
