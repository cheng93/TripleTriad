using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TripleTriad.BackgroundTasks.Queue;

namespace TripleTriad.BackgroundTasks
{
    public class MediatorHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue queue;
        private readonly ILogger<MediatorHostedService> logger;
        private readonly IServiceProvider serviceProvider;
        public MediatorHostedService(IBackgroundTaskQueue queue, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            this.serviceProvider = serviceProvider;
            this.queue = queue;
            this.logger = loggerFactory.CreateLogger<MediatorHostedService>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Mediator Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var notification = await this.queue.DequeueAsync(stoppingToken);
                    using (var scope = this.serviceProvider.CreateScope())
                    {
                        var mediator =
                            scope.ServiceProvider
                                .GetRequiredService<IMediator>();

                        await mediator.Publish(notification, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occured");
                }
            }

            this.logger.LogInformation("Mediator Hosted Service stopped.");
        }
    }
}
