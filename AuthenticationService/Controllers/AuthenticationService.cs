using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController:ControllerBase{
        public AuthenticationController()
        {
            
        }

        [HttpGet]
        public ActionResult Test(){
            return Ok("yes");
        }
    }
}