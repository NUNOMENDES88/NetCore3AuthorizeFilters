using DemoRoles.Objects.Configurations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemoRoles.Services.Interfaces;

namespace DemoRoles.Services.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly TokenValidationObject _tokenValidationObject;

        public SecurityService(TokenValidationObject tokenValidationObject)
        {
            _tokenValidationObject = tokenValidationObject;
        }

        public string GenerateToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenValidationObject.ValidIssuerSigningKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var handler = new JwtSecurityTokenHandler();

            var tokenHandler = new JwtSecurityToken(
                issuer: _tokenValidationObject.ValidIssuer,
                audience: _tokenValidationObject.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(_tokenValidationObject.ExpireMinutes),
                claims: FakeClaims(),
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenHandler);
            return token;
        }

        public List<Claim> FakeClaims()
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Top User"),
                new Claim(ClaimTypes.Email, "topuser@a.pt"),
                new Claim(ClaimTypes.Role, "Role1"),
                new Claim(ClaimTypes.Role, "Role2"),
                new Claim("ReportIds","1"),
                new Claim("ReportIds","10")
            };
        }


    }
}
