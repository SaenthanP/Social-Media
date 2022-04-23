using FeedService.Models;
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
        public void AddFollowToCache(PublishedNetworkModel publishedNetworkModel)
        {
            _networkService.AddFollowToCache(publishedNetworkModel);
            ReBuildHomeFeed(publishedNetworkModel.UserId);
        }

        public void AddPostToCache(PublishedPostModel publishedPostModel)
        {
            _postService.AddPostToFeed(publishedPostModel);
        }

        public void ReBuildHomeFeed(string userId)
        {
           _postService.ReBuildHomeFeed(userId);
        }

        public void RemoveFollowFromCache(PublishedNetworkModel publishedNetworkModel)
        {
            _networkService.RemoveFollowFromCache(publishedNetworkModel);
        }
    }
}