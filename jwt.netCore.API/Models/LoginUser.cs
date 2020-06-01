using System.ComponentModel.DataAnnotations;

namespace jwt.netCore.API.Models
{
    public sealed class LoginUser
    {
        [Required]
        [Display(Name = "User name or E-mail")]
        public string LoginOrEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
