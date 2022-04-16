using Microsoft.EntityFrameworkCore;
using PostService.Models;

namespace PostService.Data{
    public class PostContext:DbContext{
        public PostContext(DbContextOptions<PostContext>opt):base(opt)
        {
            
        }
        public DbSet <Post>Post {get;set;}
    }
}