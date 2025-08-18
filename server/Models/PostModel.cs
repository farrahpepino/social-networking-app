namespace server.Models
{
    public class PostModel{
        public required Guid Id {get; set;}
        public required string AuthorId {get; set;}
        public required string Content {get; set;}
        public required DateTime CreatedAt {get; set;}
    }
}