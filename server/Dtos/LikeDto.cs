namespace server.Dtos{

    public class LikeDto{
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public string PostId {get; set;} = string.Empty;
        public string LikerId {get; set;} = string.Empty;
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public string Username {get; set;} = string.Empty;
    }
}

