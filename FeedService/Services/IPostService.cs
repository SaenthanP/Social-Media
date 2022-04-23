using System.Collections.Generic;
using FeedService.Dtos;

namespace FeedService.Services{
    public interface IPostService{
        void AddPostToFeed(PublishedPostDto publishedPostDto);
        IEnumerable<PublishedPostDto> GetHomeFeedByUserId(string userId);
        IEnumerable<PublishedPostDto> GetUserFeedByUserId(string userId);
        void ReBuildHomeFeed(string userId);
    }
}