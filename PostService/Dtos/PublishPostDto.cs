namespace PostService.Dtos{
    public class PublishPostDto{

        public string PostId { get; set; }

        public string UserId { get; set; }
        public string PostMessage { get; set; }
        public string PostDateTime { get; set; }
        public string EventType { get; set; }
    }
}