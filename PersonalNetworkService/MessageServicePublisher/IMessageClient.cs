using PersonalNetworkService.Dtos;
using PersonalNetworkService.Models;

namespace PersonalNetworkService.MessageServicePublisher{
    public interface IMessageClient{
        void FollowUser(PublishFeedModel publishFeedModel);
    }
}