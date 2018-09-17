using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data;
using TripleTriad.Data.Entities;

namespace TripleTriad.Web.Middlewares
{
    public class PlayerIdMiddleware
    {
        private readonly RequestDelegate _next;

        public PlayerIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, TripleTriadDbContext context)
        {
            var playerId = httpContext.Session.GetString("PlayerId");

            if (!Guid.TryParse(playerId, out _))
            {
                var playersCount = await context.Players.CountAsync();
                var player = new Player()
                {
                    DisplayName = $"Guest{playersCount}"
                };
                await context.AddAsync(player);
                await context.SaveChangesAsync();

                httpContext.Session.SetString("PlayerId", player.PlayerId.ToString());
            }

            await _next(httpContext);
        }
    }
}