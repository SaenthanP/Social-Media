using System;
using System.Threading.Tasks;
using AuthenticationService.Data;
using AuthenticationService.Dtos;
using AuthenticationService.Messaging;
using AuthenticationService.Models;
using AuthenticationService.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase{
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IMessageClient _messageclient;

        public AuthenticationController(IUserRepo userRepo, IMapper mapper, IJwtService jwtService,IMessageClient messageClient)
        {
            _userRepo=userRepo;
            _mapper=mapper;
            _jwtService=jwtService;
            _messageclient=messageClient;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]        
        [HttpGet]
        public ActionResult Test(){
            return Ok("yes");
        } 

        [HttpGet("{email}",Name="GetUserByEmail")]
        public ActionResult<ReadUserDto> GetUserByEmail(string email){
            var user=_userRepo.GetUserByEmail(email);
            if(user==null){
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost("{login}")]
        public ActionResult<User> Login(LoginUserDto loginUserDto){
            if(string.IsNullOrEmpty(loginUserDto.Username) || string.IsNullOrEmpty(loginUserDto.Password)){
                 ModelState.AddModelError("Credentials Empty","Please fill out username and password");
                return Unauthorized(ModelState.Values);
            }

            if(!_userRepo.IsUserMatch(loginUserDto)){
                ModelState.AddModelError("Invalid Login","Please check your credentials");
                return Unauthorized(ModelState.Values);
            }
            
            var user=_userRepo.GetUserByUsername(loginUserDto.Username);
            var token=_jwtService.IssueToken(user);
            var output=new {
                token
            };            

            return Ok(output);
        }

        [HttpPost]
        public async Task<ActionResult<ReadUserDto>> CreateUser(CreateUserDto createUser){
            
            if(_userRepo.IsEmailExists(createUser.Email)){
               ModelState.AddModelError("Email","User with this email is already registered");
               
               return BadRequest(ModelState);
            }
            
            if(!_userRepo.IsEmailValid(createUser.Email)){
                ModelState.AddModelError("Email","Not a valid email");
              
               return BadRequest(ModelState);
            }

            if(_userRepo.IsUsernameExists(createUser.Username)){
                ModelState.AddModelError("Username","User with this username is already register");
              
               return BadRequest(ModelState);
            }

            if(createUser.ConfirmPassword!=createUser.Password){
                ModelState.AddModelError("Password","Password Mismatch");
              
               return BadRequest(ModelState);
            }

             if(createUser.Password.Length<8){
                ModelState.AddModelError("Password","Password must be atleast 8 characters");
              
               return BadRequest(ModelState);
            }

           _userRepo.CreateUser(createUser);
           _userRepo.SaveChanges();
           try{
            _messageclient.SendEmail("Testing creating user email");

           }catch(Exception ex){

           }
            var userModel=_mapper.Map<ReadUserDto>(_userRepo.GetUserByEmail(createUser.Email));
            return CreatedAtRoute(nameof(GetUserByEmail),new {email=userModel.Email},userModel);
        }
    }
}