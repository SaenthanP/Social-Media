using System;
using AuthenticationService.Data;
using AuthenticationService.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase{
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public AuthenticationController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo=userRepo;
            _mapper=mapper;
        }

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

        [HttpPost]
        public ActionResult<ReadUserDto> CreateUser(CreateUserDto createUser){
            
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
            
            var userModel=_mapper.Map<ReadUserDto>(_userRepo.GetUserByEmail(createUser.Email));
            return CreatedAtRoute(nameof(GetUserByEmail),new {email=userModel.Email},userModel);
        }
    }
}