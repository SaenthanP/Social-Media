using FeedService.Dtos;

namespace FeedService.Events{
    public interface IEventProcessing{
        void AddFollowToCache(PublishedNetworkDto publishedNetworkDto);
        
        void AddPostToCache(PublishedPostDto publishedPostDto);
    }
}