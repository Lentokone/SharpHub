using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharpHub.Models
{
    public class CreateRepositoryViewModel
    {
        [Required]
        [DisplayName("Repository Name")]
        public string RepositoryName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; } = "";

        public CreateRepositoryViewModel()
        {
            RepositoryName = string.Empty;
        }
    }
}
