using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripleTriad.Common;
using TripleTriad.Requests.GameRequests;
using TripleTriad.Requests.HubRequests;
using TripleTriad.Web.Extensions;
using TripleTriad.Web.Models;

namespace TripleTriad.Web.Controllers
{
    [ApiController]
    [Route("api/games")]
    [Authorize(Policy = Constants.TripleTriad)]
    public class GameController : Controller
    {
        private readonly IMediator mediator;

        public GameController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("")]
        [AllowAnonymous]
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

        [HttpPut("{gameId}/view")]
        public async Task<IActionResult> View(int gameId)
        {
            var playerId = base.HttpContext.GetPlayerId();
            var notifyRequest = new GameHubUserNotify.Request()
            {
                GameId = gameId,
                UserId = playerId.ToString()
            };
            await this.mediator.Send(notifyRequest, default);

            var request = new GameView.Request()
            {
                GameId = gameId,
                PlayerId = playerId
            };

            var response = await this.mediator.Send(request, default);

            return base.Json(new
            {
                IsHost = response.IsHost,
                IsPlayerTwo = response.IsPlayerTwo
            });
        }
    }
}