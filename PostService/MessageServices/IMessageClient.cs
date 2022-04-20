using PostService.Dtos;

namespace PostService.MessageServices{
    public interface IMessageClient{
        void PostCreated(PublishPostDto publishPostDto);
    }
}