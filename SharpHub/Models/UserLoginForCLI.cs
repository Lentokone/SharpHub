namespace SharpHub.Models
{
    public record UserLoginForCLI
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string SSHKey { get; set; }

        public UserLoginForCLI(string username, string password, string sshkey)
        {
            Username = username;
            Password = password;
            SSHKey = sshkey;
        }

        public UserLoginForCLI()
        {
            Username = string.Empty;
            Password = string.Empty;
            SSHKey = string.Empty;
        }
    }
}
