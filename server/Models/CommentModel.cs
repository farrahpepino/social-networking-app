namespace server.Models{

    public class CommentModel{
        public required string PostAuthorId {get; set;}
        public required string AuthorId {get; set;}
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public required string Content {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
    }


}