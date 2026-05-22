using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharpHub.Models
{
    public class UserRegisterViewModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(256)]
        [DisplayName("Username")]
        [RegularExpression("^[a-zA-Z0-9_-]{1,100}$", ErrorMessage = "Name has invalid characters in it")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool? RegisterStatus { get; set; }

        public UserRegisterViewModel()
        {
            RegisterStatus = true;
        }
    }
}
