namespace server.Models{
    public class Interest{
        public string Id {get; set;}
        public string UserId1 {get; set;}
        public string UserId2 {get; set;}
        public required DateTime CreatedAt {get; set;} 
    }
}

//For future use


//CREATE TRIGGER ADD TO LIST OF FOLLOWINGS;
