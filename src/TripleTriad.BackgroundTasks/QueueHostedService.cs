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
        private CancellationTokenSource shutdown = new CancellationTokenSource();
        private Task backgroundTask;

        public QueueHostedService(IBackgroundTaskQueue queue)
        {
            this.queue = queue;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!this.shutdown.IsCancellationRequested)
            {
                var task = await this.queue.DequeueAsync(this.shutdown.Token);
                try
                {
                    await task(this.shutdown.Token);

                }
                catch (Exception)
                {
                    // Maybe I should log something here...
                }
            }
        }
    }
}
