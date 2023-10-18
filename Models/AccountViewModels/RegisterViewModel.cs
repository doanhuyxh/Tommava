using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Tommava.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; }
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PasswordHash", ErrorMessage = "Mật khẩu không khớp")]
        public string? ConfirmPassword { get; set; }

        public static implicit operator ApplicationUser(RegisterViewModel vm)
        {
            return new ApplicationUser
            {
                UserName = vm.UserName,
                FullName = vm.FullName,
                Email = vm.Email,
                Avatar = "/upload/avatar/blank_avatar.png",
				IsActive = true,
            };
        }
    }
}