using FeedService.Dtos;
using FeedService.Services;

namespace FeedService.Events{
    public class EventProcessing : IEventProcessing
    {
        private readonly IPostService _postService;
        private readonly INetworkService _networkService;

        public EventProcessing(IPostService postService,INetworkService networkService)
        {
            _postService=postService;
            _networkService=networkService;
        }
        public void AddFollowToCache(PublishedNetworkDto publishedNetworkDto)
        {
            _networkService.AddFollowToCache(publishedNetworkDto);
            ReBuildHomeFeed(publishedNetworkDto.UserId);
        }

        public void AddPostToCache(PublishedPostDto publishedPostDto)
        {
            _postService.AddPostToFeed(publishedPostDto);
        }

        public void ReBuildHomeFeed(string userId)
        {
           _postService.ReBuildHomeFeed(userId);
        }
    }
}