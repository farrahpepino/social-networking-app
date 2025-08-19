namespace server.Models{

    public class RegistrationModel{
        public required string Email {get; set;}
        public required string Username {get; set;}
        public required string Password {get; set;}
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public required DateTime CreatedAt {get; set;} = DateTime.Now;

    }
}