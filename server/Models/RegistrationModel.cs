namespace server.Models{
    public class RegistrationModel{
        public required string Username {get; set;}
        public required string HashedPassword { get; set; }
        public required string Email {get; set;}
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public DateTime CreatedAt {get; set;} = DateTime.Now;
    }
}