namespace SharpHub.Models
{
    public class User : DB_SaveableObject
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public User() { }
    }
}
