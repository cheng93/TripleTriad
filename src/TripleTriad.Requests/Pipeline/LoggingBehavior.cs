using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.Requests.Logging;

namespace TripleTriad.Requests.Pipeline
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private const string MessageTemplate = "Handling {RequestHandler} {Outcome} in {Elapsed:0.0000} ms";

        private const string MessageTemplateDebug = "Handling {RequestHandler} with {@Request} {Outcome} in {Elapsed:0.0000} ms";

        private readonly ILog logger;

        public LoggingBehavior()
        {
            logger = LogProvider.For<TRequest>();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var start = Stopwatch.GetTimestamp();
            try
            {
                var response = await next();
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
                this.Log(request, elapsedMs);
                return response;
            }
            // Never caught, because `LogException()` returns false.
            catch (Exception ex) when (this.LogException(request, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex)) { throw; }
        }

        private void Log(TRequest request, double elapsedMs, Exception ex = null)
        {
            var message = MessageTemplate;
            var args = new List<object>() { typeof(TRequest).FullName };
            if (this.logger.IsDebugEnabled())
            {
                message = MessageTemplateDebug;
                args.Add(request);

            }

            args.Add(ex == null ? "completed" : "errored");
            args.Add(elapsedMs);

            if (ex == null)
            {
                this.logger.Info(message, args.ToArray());
            }
            else
            {
                this.logger.ErrorException(message, ex, args.ToArray());
            }
        }

        private bool LogException(TRequest request, double elapsedMs, Exception ex)
        {
            this.Log(request, elapsedMs, ex);

            return false;
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}