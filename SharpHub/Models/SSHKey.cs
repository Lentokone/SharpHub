namespace SharpHub.Models
{
    public class SSHKey : DB_SaveableObject
    {
        public string UserID { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
public DateTime CreatedAt { get; set; }
        public SSHKey() { }
    }
}
