namespace SharpHub.Models
{
    public class CreateRepoInput(string repositoryName, string owner, string description)
    {
        public string RepositoryName { get; set; } = repositoryName;
        public string Owner { get; set; } = owner;
        public string Description { get; set; } = description ?? "";
    }
}
