using DemoRoles.Services.Interfaces;
using DemoRoles.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DemoRoles.Services
{
    public static class DemoRolesServicesIOC
    {
        public static void AddCustomServicesIoc(this IServiceCollection services)
        {
            services.AddScoped<ISecurityService, SecurityService>();

        }
    }
}
