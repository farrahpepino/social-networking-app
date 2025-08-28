using System;
using System.Text.Json.Serialization;

namespace server.Models
{

    public class Post{
        public string Id {get; set;} = Guid.NewGuid().ToString();
        public required string AuthorId {get; set;} = string.Empty;
        public required string Content {get; set;} = string.Empty;
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public string Username {get; set;} = string.Empty;
    }

}
