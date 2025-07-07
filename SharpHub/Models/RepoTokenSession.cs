namespace SharpHub.Models
{
    public class RepoTokenSession : DB_SaveableObject
    {
        public string UserId { get; set; }
        public string RepoId { get; set; }
        public string RefreshToken { get; set; }
        public bool IsActive { get; set; }
        public JwtTokenInfo? JwtToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUsedAt { get; set; }
    }
}
