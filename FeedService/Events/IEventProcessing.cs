using FeedService.Models;

namespace FeedService.Events{
    public interface IEventProcessing{
        void AddFollowToCache(PublishedNetworkModel publishedNetworkModel);
        void RemoveFollowFromCache(PublishedNetworkModel publishedNetworkModel);
        
        void AddPostToCache(PublishedPostModel publishedPostModel);
        void ReBuildHomeFeed(string userId);
    }
}