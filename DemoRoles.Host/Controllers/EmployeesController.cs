using DemoRoles.Host.Classes;
using DemoRoles.Host.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoRoles.Host.Controllers
{

    [Authorize]
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IReportValidation _reportValidation;

        public EmployeesController(IReportValidation reportValidation)
        {
            _reportValidation = reportValidation;
        }

        /// <summary>
        /// This method uses the JwtBearerEvents to validate roles in the moment that it receives the token in request
        /// </summary>
        /// <returns>Return the string</returns>
        [HttpGet ("GetFilterByJwToken")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetFilterByJwToken()
        {
            return Ok("Filter By JWToken");
        }

        /// <summary>
        /// This method uses the Authorize to validate roles in claims
        /// </summary>
        /// <returns>Return the string</returns>
        [Authorize(Roles = "Role2")]
        [HttpGet("GetFilterByRole")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetFilterByRole()
        {
            return Ok("Filter By Role ");
        }


        /// <summary>
        /// This method uses the Policies to validate roles in claims
        /// </summary>
        /// <returns>Return the string</returns>
        [Authorize(Policy = "PolicyRequireRole")]
        [HttpGet("GetFilterByPolicy")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetFilterByPolicy()
        {
            return Ok("Filter By Policy");
        }


        /// <summary>
        /// This method uses the Action Filter to validate roles in claims
        /// </summary>
        /// <returns>Return the string</returns>
        [RolesFilter("Role2")]
        [HttpGet("GetFilterByRolesInActionFilter")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetFilterByRolesInActionFilter()
        {
             return Ok("Filter By Action Filter - Roles");
        }

        /// <summary>
        /// This method uses the Action Filter to validate reportId in claims
        /// </summary>
        /// <returns>Return the string</returns>
        [ReportFilter]
        [HttpGet("GetFilterByRequestValueInActionFilter/{reportId:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetFilterByRequestValueInActionFilter([FromRoute] int reportId)
        {
            return Ok("Filter By Action Filter - Request Value");
        }

    }
}