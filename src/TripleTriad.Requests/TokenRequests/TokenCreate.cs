using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TripleTriad.Common;
using TripleTriad.Requests.Jwt;
using TripleTriad.Requests.Pipeline;

namespace TripleTriad.Requests.TokenRequests
{
    public static class TokenCreate
    {
        public static SymmetricSecurityKey Key { get; }
            = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());

        public class Response
        {
            public string Token { get; set; }
        }

        public class Request : IRequest<Response>
        {
            public Guid PlayerId { get; set; }
        }

        public class Validator : ValidationPreProcessor<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.PlayerId).NotEmpty();
            }
        }

        public class RequestHandler : IRequestHandler<Request, Response>
        {
            private readonly IJwtWriter jwtWriter;
            private readonly IJwtSigningKeyProvider jwtSigningKeyProvider;

            public RequestHandler(
                IJwtWriter jwtWriter,
                IJwtSigningKeyProvider jwtSigningKeyProvider)
            {
                this.jwtWriter = jwtWriter;
                this.jwtSigningKeyProvider = jwtSigningKeyProvider;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var key = await this.jwtSigningKeyProvider.GetKey(cancellationToken);
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(Constants.Claims.PlayerId, request.PlayerId.ToString()));

                var token = new JwtSecurityToken(
                    Constants.TripleTriad,
                    Constants.TripleTriad,
                    identity.Claims,
                    signingCredentials: credentials);

                var tokenHandler = new JwtSecurityTokenHandler();

                return new Response
                {
                    Token = tokenHandler.WriteToken(token)
                };
            }
        }
    }
}
