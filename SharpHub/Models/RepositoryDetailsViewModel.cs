namespace SharpHub.Models
{
    public class RepositoryDetailsViewModel
    {
        public required string Username { get; set; }
        public required Repository CurrentRepository { get; set; }
    }
}