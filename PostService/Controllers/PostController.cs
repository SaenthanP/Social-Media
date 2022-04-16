using System;
using Microsoft.AspNetCore.Mvc;
using PostService.Data;
using PostService.Dtos;
using PostService.Models;

namespace PostService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController:ControllerBase{
        private readonly IPostRepo _repo;

        public PostController(IPostRepo repo)
        {
            _repo=repo;
        }

        [HttpPost]
        public ActionResult<Post> CreatePost(CreatePostDto createPostDto,[FromHeader(Name = "id")] string userId){
            if(string.IsNullOrEmpty(userId)){
                ModelState.AddModelError("Post","There is no user id associated with the post");
                return BadRequest(ModelState);
            }            
            
            createPostDto.PostDateTime=DateTime.Now;
            createPostDto.UserId=userId;

            var createdPostId=_repo.CreatePost(createPostDto);
            return CreatedAtRoute(nameof(GetPostById),new {id=createdPostId.Id},createdPostId);
        }

        [HttpGet("{id}", Name="GetPostById")]
        public ActionResult<Post> GetPostById(string id){
            var post=_repo.GetPostById(id);

            if(post==null){
                return NotFound();
            }

            return Ok(post);
        }
    }
}