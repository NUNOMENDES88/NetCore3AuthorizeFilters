using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace DemoRoles.Host.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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
    }
}
