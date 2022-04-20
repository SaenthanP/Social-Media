using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PostService.Data;
using PostService.Dtos;
using PostService.EventConstants;
using PostService.MessageServices;
using PostService.Models;

namespace PostService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController:ControllerBase{
        private readonly IPostRepo _repo;
        private readonly IMessageClient _messageClient;
        private readonly IMapper _mapper;

        public PostController(IPostRepo repo,IMessageClient messageClient,IMapper mapper)
        {
            _repo=repo;
            _messageClient=messageClient;
            _mapper=mapper;
        }

        [HttpPost]
        public ActionResult<Post> CreatePost(CreatePostDto createPostDto,[FromHeader(Name = "id")] string userId){
            if(string.IsNullOrEmpty(userId)){
                ModelState.AddModelError("Post","There is no user id associated with the post");
                return BadRequest(ModelState);
            }            
            
            createPostDto.PostDateTime=DateTime.Now;
            createPostDto.UserId=userId;

            var createdPost=_repo.CreatePost(createPostDto);

            var publishPostDto=_mapper.Map<PublishPostDto>(createdPost);
            publishPostDto.EventType=PostConstants.CREATE_POST;
            _messageClient.PostCreated(publishPostDto);

            return CreatedAtRoute(nameof(GetPostById),new {id=createdPost.Id},createdPost);
        }

        [HttpGet("{id}", Name="GetPostById")]
        public ActionResult<Post> GetPostById(string id){
            var post=_repo.GetPostById(id);

            if(post==null){
                return NotFound();
            }

            return Ok(post);
        }

        
        [HttpGet]
        public ActionResult Test(){
            
            return Ok();
        }
    }
}