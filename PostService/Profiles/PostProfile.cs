using AutoMapper;
using PostService.Dtos;
using PostService.Models;

namespace PostService.Profiles{
    public class PostProfile:Profile{
        public PostProfile()
        {
            CreateMap<CreatePostDto,Post>()
                .ForMember(dest=>dest.PostDateTime,opt=>opt.MapFrom(src=>src.PostDateTime.ToString()));
            CreateMap<Post,PublishPostDto>()
                .ForMember(dest=>dest.PostId,opt=>opt.MapFrom(src=>src.Id));
        }
    }
}