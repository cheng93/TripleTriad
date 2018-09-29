using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Web.Constants;
using TripleTriad.Web.Extensions;
using TripleTriad.Web.Models;

namespace TripleTriad.Web.Controllers
{
    [ApiController]
    [Route("api/game")]
    [Authorize(Policy = TokenConstants.TripleTriadScheme)]
    public class GameController : Controller
    {
        private readonly IMediator mediator;

        public GameController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            var request = new GameList.Request();

            var response = await this.mediator.Send(request, default);
            return base.Json(new { GameIds = response.GameIds });
        }

        [HttpPost("")]
        public async Task<IActionResult> Create()
        {
            var playerId = base.HttpContext.GetPlayerId();
            var request = new GameCreate.Request()
            {
                PlayerId = playerId
            };
            var response = await this.mediator.Send(request, default);
            return base.Json(new { GameId = response.GameId });
        }

        [HttpPut("{gameId}/join")]
        public async Task<IActionResult> Join(int gameId)
        {
            var playerId = base.HttpContext.GetPlayerId();
            var request = new GameJoin.Request()
            {
                GameId = gameId,
                PlayerId = playerId
            };
            var response = await this.mediator.Send(request, default);
            return base.Json(new { GameId = response.GameId });
        }

        [HttpPut("{gameId}/selectcards")]
        public async Task<IActionResult> SelectCards(int gameId, [FromBody]IEnumerable<string> cards)
        {
            var playerId = base.HttpContext.GetPlayerId();
            var request = new CardSelect.Request()
            {
                GameId = gameId,
                PlayerId = playerId,
                Cards = cards
            };
            var response = await this.mediator.Send(request, default);
            return base.Json(new { GameId = response.GameId, Cards = response.Cards });
        }

        [HttpPut("{gameId}/move")]
        public async Task<IActionResult> Move(int gameId, [FromBody]GameMoveModel move)
        {
            var playerId = base.HttpContext.GetPlayerId();
            var request = new GameMove.Request()
            {
                GameId = gameId,
                PlayerId = playerId,
                Card = move.Card,
                TileId = move.TileId
            };
            var response = await this.mediator.Send(request, default);
            return base.Json(new { GameId = response.GameId, Cards = response.Cards, Tiles = response.Tiles });
        }
    }
}