using DemoRoles.Objects.Configurations;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace DemoRoles.Host.Extensions
{
    public static class ReportExtension
    {
        public static void ValidateRoles(
            this ReportObject report,
            ClaimsPrincipal user)
        {
            var myRolesClaim = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            //Not intersect one off valid roles
            if (!myRolesClaim.Intersect(report.Roles).Any())
            {
                throw new UnauthorizedAccessException("Do not contains at least one valid role.");
            }
        }




    }
}
