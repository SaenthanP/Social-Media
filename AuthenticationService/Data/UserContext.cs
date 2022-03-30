using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Data{
    public class UserContext:DbContext{
        public UserContext(DbContextOptions<UserContext>opt):base(opt)
        {
            
        }

        public DbSet<User>User{get;set;}
    }
} 