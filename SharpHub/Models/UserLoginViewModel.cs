using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharpHub.Models
{
    public class UserLoginViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string? Username { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
