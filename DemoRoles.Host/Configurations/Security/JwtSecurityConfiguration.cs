using DemoRoles.Objects.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace DemoRoles.Host.Configurations.Security
{
    public static class JwtSecurityConfiguration
    {
        public static void AddCustomSecurity(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            //Get the validations in appsettings
            TokenValidationObject tokenValidationObject = configuration.GetSection("TokenValidations").Get<TokenValidationObject>();
            services.AddSingleton(tokenValidationObject);

            var key = Encoding.UTF8.GetBytes(tokenValidationObject.ValidIssuerSigningKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.SaveToken = tokenValidationObject.SaveToken;
                options.RequireHttpsMetadata = tokenValidationObject.RequireHttpsMetadata;
                options.IncludeErrorDetails = tokenValidationObject.IncludeErrorDetails;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenValidationObject.ValidIssuer,
                    ValidateIssuer = tokenValidationObject.ValidateIssuer,

                    ValidAudience = tokenValidationObject.ValidAudience,
                    ValidateAudience = tokenValidationObject.ValidateAudience,

                    ValidateLifetime = tokenValidationObject.ValidateLifetime,

                    ValidateIssuerSigningKey =  tokenValidationObject.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

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
            });
        }

        public static void AddPolicies(this IServiceCollection services)
        {

            services.AddAuthorization(options => 
                { options.AddPolicy("PolicyRequireRole", policy => policy.RequireRole("Role2")); }
                );

        }


    }
}
