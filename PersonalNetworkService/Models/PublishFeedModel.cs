namespace PersonalNetworkService.Models{
    public class PublishFeedModel{
        public string UserId { get; set; }
        public string Username { get; set; }

        public string UserToFollowId { get; set; }
        public string UserToFollowName { get; set; }
        public string EventType { get; set; }
    }
}