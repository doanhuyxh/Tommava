using Microsoft.AspNetCore.Identity;

namespace Tommava.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Avatar { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? About { get; set; } = string.Empty;
        public DateTime? DateVipCreate { get; set; }
        public int? VipId { get; set; }
        public DateTime? DateVipEnd { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
