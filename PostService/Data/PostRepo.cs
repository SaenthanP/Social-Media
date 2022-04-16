using System;
using System.Linq;
using AutoMapper;
using PostService.Dtos;
using PostService.Models;

namespace PostService.Data{
    public class PostRepo : IPostRepo
    {
        private readonly PostContext _context;
        private readonly IMapper _mapper;

        public PostRepo(PostContext context,IMapper mapper)
        {
            _context=context;
            _mapper=mapper;
        }

        public Post CreatePost(CreatePostDto createPostDto)
        {
            createPostDto.PostDateTime=DateTime.UtcNow;
            var postToAdd=_mapper.Map<Post>(createPostDto);
            _context.Add(postToAdd);
            SaveChanges();
            return postToAdd;
        }

        public Post GetPostById(string id)
        {
            return _context.Post.FirstOrDefault(p=>p.Id==id);
        }

        public bool SaveChanges()
        {
           return _context.SaveChanges()>=0;
        }
    }
}