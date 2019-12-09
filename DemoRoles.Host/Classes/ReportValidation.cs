using System;
using System.Linq;
using System.Security.Claims;
using DemoRoles.Objects.Configurations;
using Microsoft.AspNetCore.Http;

namespace DemoRoles.Host.Classes
{
    public interface IReportValidation
    {
        void ValidateRoles(ReportObject report);
    }

    public class ReportValidation : IReportValidation
    {
        private readonly IHttpContextAccessor _context;

        public ReportValidation(IHttpContextAccessor context)
        {
            _context = context;
        }

        public void ValidateRoles(ReportObject report)
        {
            var claimsUser = _context.HttpContext.User.Claims;
            var myRolesClaim = claimsUser
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
