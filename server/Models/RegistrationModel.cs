namespace server.Models{

    public class RegistrationModel{

        public required string Email {get; set;}
        public required string Username {get; set;}
        public required string Password {get; set;}
        public required string ConfirmPassword {get; set;}
        public required DateTime Timestamp {set;}

    }
}