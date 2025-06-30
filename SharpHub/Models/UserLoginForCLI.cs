namespace SharpHub.Models
{
    public class UserLoginForCLI
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepositoryName { get; set; } = string.Empty;
        public UserLoginForCLI(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public UserLoginForCLI()
        {
            // Default constructor for deserialization or other purpose
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}
