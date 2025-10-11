namespace SharpHub.Models
{
    public class RepositoryManagerViewModel
    {
        public string Username { get; set; }

        public bool IsRepoDetails { get; set; } = false;
        
        public RepositoryDetailsViewModel RepositoryDetailsViewModel { get; set; }
        public RepositoryListViewModel RepositoryListViewModel { get; set; }
    }
}