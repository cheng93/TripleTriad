using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TripleTriad.Requests.Jwt
{
    public class JwtWriter : IJwtWriter
    {
        public string Write(SecurityToken token)
            => new JwtSecurityTokenHandler().WriteToken(token);
    }
}