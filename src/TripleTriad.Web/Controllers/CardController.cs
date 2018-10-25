using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripleTriad.Requests.CardRequests;

namespace TripleTriad.Web.Controllers
{
    [ApiController]
    [Route("api/cards")]
    [AllowAnonymous]
    public class CardController : Controller
    {
        private readonly IMediator mediator;

        public CardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var request = new AllCardsList.Request();
            var response = await this.mediator.Send(request);

            return this.Json(new { Cards = response.Cards });
        }
    }
}