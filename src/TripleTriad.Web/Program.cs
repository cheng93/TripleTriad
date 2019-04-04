using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace TripleTriad.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    var levelSwitch = new LoggingLevelSwitch();

                    loggerConfiguration
                        .MinimumLevel.ControlledBy(levelSwitch)
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Error)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Application", "TripleTriad")
                        .WriteTo.Console();

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerConfiguration.WriteTo.Seq(
                            "http://localhost:5341",
                            controlLevelSwitch: levelSwitch);
                    }
                })
                .UseStartup<Startup>();
    }
}
