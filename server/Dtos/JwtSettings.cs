namespace server.Dtos{
    public class JwtSettings{
        public required string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}