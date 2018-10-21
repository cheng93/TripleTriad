using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TripleTriad.Requests.TokenRequests;
using TripleTriad.SignalR.Constants;

namespace TripleTriad.Web.Extensions
{
    public static class AuthenticationServiceCollectionExtension
    {
        public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(AuthConstants.TripleTriadScheme, policy =>
                    {
                        policy
                            .AddAuthenticationSchemes(AuthConstants.TripleTriadScheme)
                            .RequireClaim(ClaimConstants.PlayerId);
                    });
                })
                .AddAuthentication(AuthConstants.TripleTriadScheme)
                .AddJwtBearer(AuthConstants.TripleTriadScheme, options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/gameHub")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
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