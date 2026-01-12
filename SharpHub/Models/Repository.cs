namespace SharpHub.Models
{
    public class Repository : DB_SaveableObject
    {
        public string RepositoryName { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public string RepositoryPath { get; set; } // Path to the repository on the server
        public bool IsDeleted { get; set; } = false; // Default is not deleted

        public DateTime EditedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        /*For later expansion
         * 
         *public string Visibility { get; set; } = "Private"; // Default visibility is Private
         *public List<string> Collaborators { get; set; } = new List<string>();
        */

        public Repository(string repositoryName, string owner, string description, string repositoryPath)
        {
            RepositoryName = repositoryName;
            Owner = owner;
            Description = description ?? "";
            RepositoryPath = repositoryPath;
            EditedAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow; // Set the creation time to now
        }
    }
}
