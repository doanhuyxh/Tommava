using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.AccountViewModels
{
    public class UserProFileViewModel
    {
        public string ApplicationUserId { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile AvatarFile { get; set; }
        public string AvatarPath { get; set; }
        [Display(Name = "Nhóm người dùng")]
        public string Role { set; get; }
        [Display(Name = "Tài khoản")]
        public string UserName { set; get; }
        public string FullName { set; get; }
        public bool IsActive { get; set; }
        public string? About { get; set; } = string.Empty;
        public DateTime? DateVipCreate { get; set; }
        public int? VipId { get; set; }
        public string? VipName { get; set; }
        public DateTime? DateVipEnd { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Email { set; get; }
        
    }
}
