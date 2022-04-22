using System.Collections.Generic;
using FeedService.Dtos;

namespace FeedService.Events{
    public interface IPostEventProcessing{
        void AddPostToFeed(PublishedPostDto publishedPostDto);
        IEnumerable<PublishedPostDto> GetHomeFeedByUserId(string userId);
    }
}