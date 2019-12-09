# Authorization in ASP.NET Core

###Simple authorization
Authorization in MVC is controlled through the AuthorizeAttribute attribute and its various parameters. At its simplest, applying the AuthorizeAttribute attribute to a controller or action limits access to the controller or action to any authenticated user.

For example, the following code limits access to the AccountController to any authenticated user.
```csharp
    [Authorize]
    [Route("api/v1/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
	
	}
```
Reference: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/simple?view=aspnetcore-3.1

###Role Authorization
Role-based authorization checks are declarative—the developer embeds them within their code, against a controller or an action within a controller, specifying roles which the current user must be a member of to access the requested resource.

For example, the following code limits access to any actions on the AdministrationController to users who are a member of the Administrator role:
```csharp
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
```
###Policy  Authorization
```csharp
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
```
### ActionFilter
```csharp
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
```

```csharp
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
```
### Middleware
```csharp
  public async Task Invoke(
            HttpContext httpContext, 
            IAuthorizationService authorizationService)
        {
            //Contains Authorization 
            if (httpContext.Request.Headers.Any(p => p.Key == "Authorization"))
            {
                var route = httpContext.GetRouteData();
                //Exist this parameter in route request
                if (route.Values.TryGetValue("reportId", out var reportIdValue))
                {
                    //Validate conditions
                }
            }
            await _next(httpContext);
        }
```
###Bearer Event
```csharp
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        //Request role claim
                        if (!context.Principal.Claims.Any(y => y.Type == ClaimTypes.Role))
                        {
                            throw new UnauthorizedAccessException("The role attribute is not present in the token.");
                        }

                        //Valid roles
                        var validRoles = tokenValidationObject.ValidRoles;

                        //Get roles
                        var myRolesClaim = context.Principal.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value);

                        //Not intersect one off valid roles
                        if (!myRolesClaim.Intersect(validRoles).Any())
                        {
                            throw new UnauthorizedAccessException("Do not contains at least one valid role.");
                        }

                        await Task.FromResult(0);
                    }
                };
```
