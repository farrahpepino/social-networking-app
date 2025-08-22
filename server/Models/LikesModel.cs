namespace server.Models{

    public class LikesModel{
        public required string Id {get; set;} = Guid.NewGuid().ToString();
        public required string PostId {get; set;} = string.Empty;
        public string LikerId {get; set;} = string.Empty;
        public DateTime CreatedAt {get; set;} = DateTime.Now;
    }
}

