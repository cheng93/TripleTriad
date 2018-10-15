using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripleTriad.Requests.GuestPlayerRequests;
using TripleTriad.Requests.TokenRequests;
using TripleTriad.Web.Constants;

namespace TripleTriad.Web.Controllers
{
    [ApiController]
    [Route("api/token")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        private readonly IMediator mediator;

        public TokenController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate()
        {
            Claim claim;
            if (!HttpContext.User.HasClaim(c => c.Type == ClaimConstants.PlayerId))
            {
                var createPlayerResponse = await this.mediator.Send(new GuestPlayerCreate.Request());
                claim = new Claim(ClaimConstants.PlayerId, createPlayerResponse.PlayerId.ToString());
            }
            else
            {
                claim = HttpContext.User.Claims.First(x => x.Type == ClaimConstants.PlayerId);
            }

            var identity = new ClaimsIdentity();
            identity.AddClaim(claim);

            var tokenCreateResponse = await this.mediator.Send(
                new TokenCreate.Request()
                {
                    ClaimsIdentity = identity
                });

            return base.Json(new { Token = tokenCreateResponse.Token });
        }
    }
}