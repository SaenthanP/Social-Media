using AuthenticationService.Dtos;
using AuthenticationService.Models;
using AutoMapper;
namespace AuthenticationService.Profiles{
    public class UserProfile:Profile{
        public UserProfile()
        {
            CreateMap<CreateUserDto,User>();
            CreateMap<User,CreateUserDto>();
            CreateMap<User,ReadUserDto>();
            CreateMap<ReadUserDto,MessageUserDto>();
        }
        
    }
}