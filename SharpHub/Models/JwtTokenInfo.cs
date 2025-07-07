namespace SharpHub.Models
{
    public class JwtTokenInfo
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
