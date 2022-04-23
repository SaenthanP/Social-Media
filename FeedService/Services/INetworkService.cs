using System.Collections.Generic;
using FeedService.Dtos;
using StackExchange.Redis;

namespace FeedService.Services{
    public interface INetworkService{
        void AddFollowToCache(PublishedNetworkDto publishedNetworkDto);
        IEnumerable<string> GetFollowerListByUserId(string userId);
        IEnumerable<string> GetFollowingListByUserId(string userId);

    }
}