namespace SharpHub.Models
{
    public class RepositoryManagerViewModel
    {
        public required string Username { get; set; }

        public bool IsRepoDetails { get; set; } = false;

        public RepositoryDetailsViewModel? RepositoryDetailsViewModel { get; set; }
        public RepositoryListViewModel? RepositoryListViewModel { get; set; }
    }
}
