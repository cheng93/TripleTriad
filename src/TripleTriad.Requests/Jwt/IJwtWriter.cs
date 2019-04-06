using Microsoft.IdentityModel.Tokens;

namespace TripleTriad.Requests.Jwt
{
    public interface IJwtWriter
    {
        string Write(SecurityToken token);
    }
}