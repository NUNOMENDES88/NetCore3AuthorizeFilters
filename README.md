# Authorization in ASP.NET Core

Authorization in MVC is controlled through the AuthorizeAttribute attribute and its various parameters.

### Simple authorization

For this example, the following code limits access to the controller to any authenticated user.

```csharp
    [Authorize]
    [Route("api/v1/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
	
    }
```

Reference: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/simple?view=aspnetcore-3.1

### Role Authorization
For this example, the following code limits access to the GetFilterByRole to users who are members of the role 'Role2'.

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

Reference:https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-3.1


### Policy  Authorization
Policies are applied to controllers by using the [Authorize] attribute with the policy name

For this example, the following code limits access to the GetFilterByPolicy to users who are a member of the Role2 .


1º Create policy in startup file
```csharp
  services.AddAuthorization(options => 
    { 
        options.AddPolicy("PolicyRequireRole", policy => policy.RequireRole("Role2")); 
    }
  );
```
2º Add Policy in header
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

Reference: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-3.1


### ActionFilter

An action filter is an attribute. You can apply most action filters to either an individual controller action or an entire controller.

#### Global Filter
1º For this example, the following code limits access to the GetFilterByRolesInActionFilter method  to users who are a member of the 'Allow Roles' .

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



#### Filter by route parameters
2º For this example, the following code limits access to the GetFilterByRolesInActionFilter method to reports authorized in claims

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


Reference: https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/understanding-action-filters-cs
Reference: https://exceptionnotfound.net/asp-net-mvc-demystified-action-filters/


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

Reference: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1

### Bearer Event

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

Reference: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer.jwtbearerevents?view=aspnetcore-2.2

