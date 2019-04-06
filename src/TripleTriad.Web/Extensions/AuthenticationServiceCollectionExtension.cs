using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TripleTriad.Common;
using TripleTriad.Requests.TokenRequests;

namespace TripleTriad.Web.Extensions
{
    public static class AuthenticationServiceCollectionExtension
    {
        public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(Constants.TripleTriad, policy =>
                    {
                        policy
                            .AddAuthenticationSchemes(Constants.TripleTriad)
                            .RequireClaim(Constants.Claims.PlayerId);
                    });
                })
                .AddAuthentication(Constants.TripleTriad)
                .AddJwtBearer(Constants.TripleTriad, options =>
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