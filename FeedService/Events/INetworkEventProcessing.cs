using FeedService.Dtos;
using StackExchange.Redis;

namespace FeedService.Events{
    public interface INetworkEventProcessing{
        void AddFollowToCache(PublishedNetworkDto publishedNetworkDto);
        RedisValue[] GetFollowListByUserId(string userId);
    }
}