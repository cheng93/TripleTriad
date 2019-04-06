using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TripleTriad.Common;

namespace TripleTriad.Requests.Jwt
{
    public class ConfigJwtSigningKeyProvider : IJwtSigningKeyProvider
    {
        private readonly IConfiguration configuration;

        public ConfigJwtSigningKeyProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task<SymmetricSecurityKey> GetKey(CancellationToken cancellationToken)
        {
            var secret = this.configuration[Constants.Config.JwtSigningKey];
            var bytes = Encoding.ASCII.GetBytes(secret);
            return Task.FromResult(new SymmetricSecurityKey(bytes));
        }
    }
}