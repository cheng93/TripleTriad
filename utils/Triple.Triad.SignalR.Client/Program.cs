using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TripleTriad.SignalR;

namespace Triple.Triad.SignalR.Client
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/gameHub")
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .AddMessagePackProtocol()
                .Build();

            await connection.StartAsync();

            Console.WriteLine("Starting connection. Press Ctrl-C to close.");
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, a) =>
            {
                a.Cancel = true;
                cts.Cancel();
            };

            connection.Closed += e =>
            {
                Console.WriteLine("Connection closed with error: {0}", e);

                cts.Cancel();
                return Task.CompletedTask;
            };

            connection.On<GameDataMessage>("Send", (message) =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(message));
            });

            while (true) { }
        }
    }
}
