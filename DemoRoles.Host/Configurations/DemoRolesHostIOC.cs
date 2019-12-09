using DemoRoles.Host.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DemoRoles.Host.Configurations
{
    public static class DemoRolesHostIOC
    {
        public static void AddCustomDemoRolesHostIOC(
            this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IReportValidation, ReportValidation>();
        }
    }
}
