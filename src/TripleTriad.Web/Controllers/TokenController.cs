using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripleTriad.Common;
using TripleTriad.Requests.GuestPlayerRequests;
using TripleTriad.Requests.TokenRequests;
using TripleTriad.Web.Extensions;

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
            Guid playerId;
            if (!base.HttpContext.User.HasClaim(c => c.Type == Constants.Claims.PlayerId))
            {
                var createPlayerResponse = await this.mediator.Send(new GuestPlayerCreate.Request());
                playerId = createPlayerResponse.PlayerId;
            }
            else
            {
                playerId = base.HttpContext.GetPlayerId();
            }

            var tokenCreateResponse = await this.mediator.Send(
                new TokenCreate.Request()
                {
                    PlayerId = playerId
                });

            return base.Json(new { Token = tokenCreateResponse.Token });
        }
    }
}