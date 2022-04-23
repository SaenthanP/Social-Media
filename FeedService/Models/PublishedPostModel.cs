using System;

namespace FeedService.Models{
    public class PublishedPostModel{
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string PostMessage { get; set; }
        public DateTime PostDateTime { get; set; }
        public string EventType { get; set; }

    }
}