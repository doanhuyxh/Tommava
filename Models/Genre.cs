using System.ComponentModel.DataAnnotations;

namespace Tommava.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
