namespace server.Models{
    public class User{
        public string? Username {get; set;}
        public string? Email {get; set;}
        public string? Id {get; set;}
        public DateTime? CreatedAt {get; set;}
    }
}


// public string Bio { get; set; }
// public string Location { get; set; }
// add list of followings with userid