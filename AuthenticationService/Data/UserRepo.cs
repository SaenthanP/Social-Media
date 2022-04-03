using System;
using System.Linq;
using System.Net.Mail;
using AuthenticationService.Dtos;
using AuthenticationService.Models;
using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;

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
        public void CreateUser(CreateUserDto createUserDto)
        {
            if (createUserDto==null){
                throw new ArgumentNullException(nameof(createUserDto));
            }
            
            var user=_mapper.Map<User>(createUserDto);
            user.Password=BCryptNet.HashPassword(user.Password);

            _context.Add(user);
        
            
        }

        public ReadUserDto GetUserByEmail(string email)
        {
            var user=_context.User.FirstOrDefault(p=>p.Email==email);
            return _mapper.Map<ReadUserDto>(user);
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