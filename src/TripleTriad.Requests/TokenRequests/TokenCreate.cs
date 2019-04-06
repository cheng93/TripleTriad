using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TripleTriad.Common;

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
            public ClaimsIdentity ClaimsIdentity { get; set; }
        }

        public class RequestHandler : RequestHandler<Request, Response>
        {
            protected override Response Handle(Request request)
            {
                var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    Constants.TripleTriad,
                    Constants.TripleTriad,
                    request.ClaimsIdentity.Claims,
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
