using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TripleTriad.BackgroundTasks.Queue;

namespace TripleTriad.BackgroundTasks
{
    public class QueueHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue queue;

        public QueueHostedService(IBackgroundTaskQueue queue)
        {
            this.queue = queue;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await this.queue.DequeueAsync(stoppingToken);
                try
                {
                    await task(stoppingToken);

                }
                catch (Exception)
                {
                    // Maybe I should log something here...
                }
            }
        }
    }
}
