namespace server.Models{

    public class CommentModel{
        public required string AuthorId {get; set;}
        public required string Content {get; set;}
        public required string PostId {get; set;}
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public DateTime CreatedAt {get; set;} = DateTime.Now;
    }

}