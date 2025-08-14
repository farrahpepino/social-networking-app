namespace server.Models
{

    public class PostModel{
        public required string Id {get; set;}
        public required string UserId {get; set;}
        public required string Content {get; set;}
        public required DateTime Timestamp {get; set;}
    }
}