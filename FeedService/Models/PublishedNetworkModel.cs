namespace FeedService.Models{
    public class PublishedNetworkModel{
        public string UserId { get; set; }
        public string Username { get; set; }

        public string UserToInteractWithId { get; set; }
        public string UserToInteractWithName { get; set; }
        public string EventType { get; set; }
    }
}