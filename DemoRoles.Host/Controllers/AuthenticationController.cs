using DemoRoles.Host.Models.Authentication;
using DemoRoles.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoRoles.Host.Controllers
{
    [AllowAnonymous]
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly ISecurityService _securityService;

        public AuthenticationController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        [HttpPost("")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult LoginUser(
            [FromBody]AuthenticationRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Exist empty required values in request");
            }
            //Validate credentials
            if (request.UserName != "test" && 
                request.Password != "test")
            {
                return NotFound("The user or password is invalid");
            }
            string token = _securityService.GenerateToken();
            return Ok(token);
        }

    }
}