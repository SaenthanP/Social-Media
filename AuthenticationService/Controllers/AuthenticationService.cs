using AuthenticationService.Data;
using AuthenticationService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase{
        private readonly IUserRepo _userRepo;

        public AuthenticationController(IUserRepo userRepo)
        {
            _userRepo=userRepo;
        }

        [HttpGet]
        public ActionResult Test(){
            return Ok("yes");
        }

        [HttpPost]
        public ActionResult<ReadUserDto> CreateUser(CreateUserDto createUser){
            
            return null;
        }
    }
}