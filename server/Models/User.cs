namespace server.Models{
    public class User {
        public string? Username {get; set;}
        public string? Email {get; set;}
        public string? Age {get; set;}
        public string? City { get; set; }
        public string? Id {get; set;}
        public List<string>? Interests { get; set; } = new List<string>();
        public DateTime? CreatedAt {get; set;}
    }
}

