using AuthenticationService.Data;
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

        [Route("TestBuild")]
        [HttpGet]
        public ActionResult TestBuild(){
            return Ok("Okay, changes applied successfully");
        }
    }
}