using PersonalNetworkService.Models;

namespace PersonalNetworkService.MessageServicePublisher{
    public interface IMessageClient{
        void FollowUser(PublishNetworkModel publishNetworkModel);
        void UnfollowUser(PublishNetworkModel publishNetworkModel);
    }
}