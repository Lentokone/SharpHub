using System.ComponentModel.DataAnnotations;

namespace SharpHub.Models
{
    public class CreateRepoInput
    {
        [Required(ErrorMessage = "Repository name is required")]
        [StringLength(100)]
        [RegularExpression("^[a-zA-Z0-9_-]{1,100}$", ErrorMessage = "Name has invalid characters in it")]
        public string RepositoryName { get; set; } = "";

        [StringLength(100)]
        public string Description { get; set; } = "";
        public string SelectedGitignore { get; set; } = "";

        public CreateRepoInput() { }
        public CreateRepoInput(string repositoryName, string description, string selectedgitignore)
        {
            RepositoryName = repositoryName;
            Description = description;
            SelectedGitignore = selectedgitignore;
        }
    }
}
