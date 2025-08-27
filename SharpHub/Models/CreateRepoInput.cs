namespace SharpHub.Models
{
    public class CreateRepoInput
    {
        public string RepositoryName { get; set; } = "";
        public string Owner { get; set; } = "";
        public string Description { get; set; } = "";

        public CreateRepoInput(string repositoryName, string owner, string description)
        {
            RepositoryName = repositoryName;
            Owner = owner;
            Description = description;
        }
    }
}
