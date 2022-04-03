using System;
using System.Linq;
using System.Net.Mail;
using AuthenticationService.Dtos;
using AutoMapper;

namespace AuthenticationService.Data{
    public class UserRepo : IUserRepo
    {
        private readonly UserContext _context;
        private readonly IMapper _mapper;

        public UserRepo(UserContext context, IMapper mapper)
        {
            _context=context;
            _mapper=mapper;
        }
        public ReadUserDto CreateUser(CreateUserDto createUserDto)
        {
            if (createUserDto==null){
                throw new ArgumentNullException(nameof(createUserDto));
            }

            _context.Add(createUserDto);
            var userToReturn=_context.User.FirstOrDefault(p=>p.Email==createUserDto.Email);

            return _mapper.Map<ReadUserDto>(userToReturn);
            
        }

        public bool IsEmailExists(string email)
        {
            var user=_context.User.FirstOrDefault(p=>p.Email==email);

            if(user==null){
                return false;
            }
            return true;
        }

        public bool IsEmailValid(string email)
        {
            try{
                MailAddress mailAddress=new MailAddress(email);
                return true;
            }catch{
                return false;
            }
        }

        public bool IsUsernameExists(string username)
        {
            var user=_context.User.FirstOrDefault(p=>p.Username==username);
            
            if(user==null){
                return false;
            }
            return true;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges()>=0;
        }
    }
}