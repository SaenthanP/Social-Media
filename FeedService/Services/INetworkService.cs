using FeedService.Dtos;
using StackExchange.Redis;

namespace FeedService.Services{
    public interface INetworkService{
        void AddFollowToCache(PublishedNetworkDto publishedNetworkDto);
        RedisValue[] GetFollowListByUserId(string userId);
    }
}