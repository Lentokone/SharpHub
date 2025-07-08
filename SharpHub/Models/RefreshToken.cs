namespace SharpHub.Models
{
    public class RefreshToken : DB_SaveableObject
    {
        public required string Token { get; set; }
        public int UserId { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public JwtTokenInfo?  Jwt { get; set; }
    }
}
