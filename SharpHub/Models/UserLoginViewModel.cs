using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharpHub.Models
{
    public class UserLoginViewModel
    {
        [Required]
        [DisplayName("Username")]
        [RegularExpression("^[a-zA-Z0-9_-]{1,100}$", ErrorMessage = "Name has invalid characters in it")]
        public string? Username { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
