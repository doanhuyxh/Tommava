using System.ComponentModel.DataAnnotations;

namespace Tommava.Models
{
    public class VipPackage
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
