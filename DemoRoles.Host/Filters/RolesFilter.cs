using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DemoRoles.Host.Filters
{
    public class RolesFilter : ActionFilterAttribute
    {
        private readonly List<string> _listRoles;

        public RolesFilter(params string[] listRoles)
        {
            _listRoles = listRoles.ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            if (actionExecutingContext.HttpContext.Request.Headers.Any(p => p.Key == "Authorization"))
            {
                //get the current content user
                var user = actionExecutingContext.HttpContext.User;
                if (!user.HasClaim(p => p.Type == ClaimTypes.Role))
                {
                    throw new UnauthorizedAccessException("The role attribute is not present in the token.");
                }

                //Get roles
                var myRolesClaim = user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);

                //Not intersect one off valid roles
                if (!myRolesClaim.Intersect(_listRoles).Any())
                {
                    throw new UnauthorizedAccessException("Do not contains at least one valid role.");
                }
            }
        }
    }
}
