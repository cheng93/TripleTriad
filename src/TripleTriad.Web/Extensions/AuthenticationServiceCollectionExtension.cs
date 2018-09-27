using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TripleTriad.Requests.TokenRequests;
using TripleTriad.Web.Constants;

namespace TripleTriad.Web.Extensions
{
    public static class AuthenticationServiceCollectionExtension
    {
        public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(TokenConstants.TripleTriadScheme, policy =>
                    {
                        policy
                            .AddAuthenticationSchemes(TokenConstants.TripleTriadScheme)
                            .RequireClaim(ClaimConstants.PlayerId);
                    });
                })
                .AddAuthentication(TokenConstants.TripleTriadScheme)
                .AddJwtBearer(TokenConstants.TripleTriadScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateActor = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = TokenCreate.Key
                    };
                });
            return services;
        }
    }
}