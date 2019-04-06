using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using TripleTriad.Requests.Jwt;

namespace TripleTriad.Web.Options
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IJwtSigningKeyProvider jwtSigningKeyProvider;

        public ConfigureJwtBearerOptions(IJwtSigningKeyProvider jwtSigningKeyProvider)
        {
            this.jwtSigningKeyProvider = jwtSigningKeyProvider;
        }
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            var signingKey = this.jwtSigningKeyProvider.GetKey(default).Result;

            options.TokenValidationParameters.IssuerSigningKey = signingKey;
        }
    }
}