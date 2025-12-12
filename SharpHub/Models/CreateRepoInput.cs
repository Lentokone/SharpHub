namespace SharpHub.Models
{
    public class CreateRepoInput
    {
        public string RepositoryName { get; set; } = "";
        public string Description { get; set; } = "";
        public string SelectedGitignore { get; set; } = "";

        public CreateRepoInput() {}
        public CreateRepoInput(string repositoryName, string description, string selectedgitignore)
        {
            RepositoryName = repositoryName;
            Description = description;
            SelectedGitignore = selectedgitignore;
        }
    }
}
