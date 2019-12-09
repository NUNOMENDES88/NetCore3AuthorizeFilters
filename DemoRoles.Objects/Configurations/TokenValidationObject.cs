using System.Collections.Generic;

namespace DemoRoles.Objects.Configurations
{
    public class TokenValidationObject
    {
        public bool RequireHttpsMetadata { get; set; }
        public bool IncludeErrorDetails { get; set; }
        public bool SaveToken { get; set; }

        public string Authority { get; set; }
        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }

        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public string ValidIssuerSigningKey { get; set; }
        public bool ValidateLifetime { get; set; }
        public int ExpireMinutes { get; set; }
        public List<string> ValidRoles { get; set; }
    }
}
