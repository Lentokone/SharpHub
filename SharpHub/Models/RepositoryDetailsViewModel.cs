using System.Diagnostics.Contracts;

namespace SharpHub.Models
{
    public class RepositoryDetailsViewModel
    {
        public required string Username { get; set; }
        public required string ObjectId { get; set; }
        public required Repository CurrentRepository { get; set; }
    }
}