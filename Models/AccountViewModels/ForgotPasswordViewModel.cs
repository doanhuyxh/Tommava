using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
