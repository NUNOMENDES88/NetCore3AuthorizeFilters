using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace DemoRoles.Host.Filters
{
    public class ReportFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            //Contains Authorization header
            if (actionExecutingContext.HttpContext.Request.Headers.Any(p => p.Key == "Authorization"))
            {
                var route = actionExecutingContext.RouteData;
                //Exist this parameter in route request
                if (route.Values.TryGetValue("reportId", out var reportIdValue))
                {
                    var user = actionExecutingContext.HttpContext.User;
                    if (!user.HasClaim(p => p.Type == "ReportIds"))
                    {
                        throw new UnauthorizedAccessException("The role attribute is not present in the token.");
                    }

                    //Get roles
                    var myReportsClaim = user.Claims
                        .Where(c => c.Type == "ReportIds")
                        .Select(c => c.Value);

                    //Not intersect one off valid roles
                    if (!myReportsClaim.Any(p => p == reportIdValue.ToString()))
                    {
                        throw new UnauthorizedAccessException("Do not contains permission s to access this report.");
                    }
                }
            }
        }

    }
}
