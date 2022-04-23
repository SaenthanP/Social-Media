using System.Collections.Generic;
using FeedService.Models;
using StackExchange.Redis;

namespace FeedService.Services{
    public interface INetworkService{
        void AddFollowToCache(PublishedNetworkModel publishedNetworkModel);
        void RemoveFollowFromCache(PublishedNetworkModel publishedNetworkModel);
        IEnumerable<string> GetFollowerListByUserId(string userId);
        IEnumerable<string> GetFollowingListByUserId(string userId);

    }
}