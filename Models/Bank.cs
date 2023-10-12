using System.ComponentModel.DataAnnotations;

namespace Tommava.Models
{
    public class Bank
    {
        [Key]
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string BankOwner { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
