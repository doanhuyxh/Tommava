using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.VipPackageVM
{
    public class VipPackageVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên gói")]
        public string? Name { get; set; }
        [Display(Name = "Giá")]
        public double Price { get; set; }
        [Display(Name = "Mô tả 1")]
        public string? Description1 { get; set; }
        [Display(Name = "Mô tả 2")]
        public string? Description2 { get; set; }
        [Display(Name = "Mô tả 3")]
        public string? Description3 { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator VipPackageVM(VipPackage vip)
        {
            return new VipPackageVM
            {
                Id = vip.Id,
                Name = vip.Name,
                Price = vip.Price,
                Description1 = vip.Description1,
                Description2 = vip.Description2,
                Description3 = vip.Description3,
                CreatedDate = vip.CreatedDate,
                IsDeleted = vip.IsDeleted,
            };
        }
        public static implicit operator VipPackage(VipPackageVM vm)
        {
            return new VipPackageVM
            {
                Id = vm.Id,
                Name = vm.Name??"",
                Price = vm.Price,
                Description1 = vm.Description1??"",
                Description2 = vm.Description2??"",
                Description3 = vm.Description3??"",
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
