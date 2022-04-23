using System.Collections.Generic;
using FeedService.Models;

namespace FeedService.Services{
    public interface IPostService{
        void AddPostToFeed(PublishedPostModel publishedPostModel);
        IEnumerable<PublishedPostModel> GetHomeFeedByUserId(string userId);
        IEnumerable<PublishedPostModel> GetUserFeedByUserId(string userId);
        void ReBuildHomeFeed(string userId);
    }
}