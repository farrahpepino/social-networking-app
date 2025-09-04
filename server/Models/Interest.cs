namespace server.Models {
    public class Interest {
        public string Id { get; set; } = string.Empty;
        public required string UserId1 { get; set; }
        public required string UserId2 { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
