using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.BankVM
{
    public class BankVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên ngân hàng")]
        public string BankName { get; set; }
        [Display(Name = "Số tài khoản")]
        public string BankNumber { get; set; }
        [Display(Name = "Chủ tài khoản")]
        public string BankOwner { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator BankVM(Bank bank)
        {
            return new BankVM
            {
                Id = bank.Id,
                BankName = bank.BankName,
                BankNumber = bank.BankNumber,
                BankOwner = bank.BankOwner,
                CreatedDate = bank.CreatedDate,
                IsDeleted = bank.IsDeleted,
                IsActive = bank.IsActive,
            };
        }
        public static implicit operator Bank(BankVM vm)
        {
            return new Bank
            {
                Id = vm.Id,
                BankName = vm.BankName,
                BankNumber = vm.BankNumber,
                BankOwner = vm.BankOwner,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
                IsActive=vm.IsActive,
            };
        }
    }
}
