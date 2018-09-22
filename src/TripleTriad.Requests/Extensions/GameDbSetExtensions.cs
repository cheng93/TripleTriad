using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TripleTriad.Data.Entities;
using TripleTriad.Requests.Exceptions;

namespace TripleTriad.Requests.Extensions
{
    internal static class GameDbSetExtensions
    {
        public async static Task<Game> GetGameOrThrowAsync(
            this DbSet<Game> games,
            int gameId,
            CancellationToken cancellationToken)
        {
            var gameExists = await games.AnyAsync(x => x.GameId == gameId, cancellationToken);
            if (!gameExists)
            {
                throw new GameNotFoundException(gameId);
            }
            return await games.SingleAsync(x => x.GameId == gameId, cancellationToken);
        }
    }
}