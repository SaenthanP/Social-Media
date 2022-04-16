using PostService.Dtos;
using PostService.Models;

namespace PostService.Data{
    public interface IPostRepo{
        Post CreatePost(CreatePostDto createPostDto);
        bool SaveChanges();
        Post GetPostById(string id);
    }
}