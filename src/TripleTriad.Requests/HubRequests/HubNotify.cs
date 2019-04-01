using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using TripleTriad.Requests.Pipeline;
using TripleTriad.SignalR;

namespace TripleTriad.Requests.HubRequests
{
    public static class HubNotify
    {
        public abstract class Request : IRequest
        {
            public string Message { get; set; }
        }

        public abstract class Validator<TRequest> : ValidationPreProcessor<TRequest>
            where TRequest : Request
        {
            protected Validator()
            {
                base.RuleFor(x => x.Message).NotEmpty();
            }
        }

        public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest>
            where TRequest : Request
        {
            public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
            {
                await this.GetGameClient(request).Send(request.Message);
                return new Unit();
            }

            protected abstract IGameClient GetGameClient(TRequest request);
        }
    }
}