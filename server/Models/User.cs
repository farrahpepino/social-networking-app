namespace server.Models{
    public class User{
        public string? Username {get; set;}
        public string? Email {get; set;}
        public string? Id {get; set;}
        public DateTime? CreatedAt {get; set;}
    }
}


// For future use since it is not a good practice to store token in a localstorage to get user data.
// Could also be used for searching users

// public string Bio { get; set; }
// public string Location { get; set; }