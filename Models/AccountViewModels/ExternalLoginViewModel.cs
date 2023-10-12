using System.ComponentModel.DataAnnotations;

namespace Tommava.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
