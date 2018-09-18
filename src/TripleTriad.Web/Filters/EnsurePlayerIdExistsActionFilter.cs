using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using TripleTriad.Commands.GuestPlayer;

namespace TripleTriad.Web.Filters
{
    public class EnsurePlayerIdExistsActionFilter : IAsyncActionFilter
    {
        private readonly IMediator mediator;

        public EnsurePlayerIdExistsActionFilter(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var playerId = context.HttpContext.Session.GetString("PlayerId");

            if (!Guid.TryParse(playerId, out _))
            {
                var createPlayerResponse = await this.mediator.Send(new GuestPlayerCreate.Command());

                context.HttpContext.Session.SetString("PlayerId", createPlayerResponse.PlayerId.ToString());
            }

            await next();
        }
    }
}