using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace TripleTriad.Requests.Jwt
{
    public interface IJwtSigningKeyProvider
    {
        Task<SymmetricSecurityKey> GetKey(CancellationToken CancellationToken);
    }
}