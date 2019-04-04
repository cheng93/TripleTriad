using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using TripleTriad.BackgroundTasks;
using TripleTriad.Data;
using TripleTriad.Requests.GuestPlayerRequests;
using TripleTriad.SignalR;
using TripleTriad.Web.Extensions;
using TripleTriad.Web.IoC;
using TripleTriad.Web.Middlewares;

namespace TripleTriad.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                //.AddMediatR(typeof(GuestPlayerCreate).GetTypeInfo().Assembly) //https://github.com/jbogard/MediatR.Extensions.Microsoft.DependencyInjection/issues/29
                .AddDbContextPool<TripleTriadDbContext>(options
                    => options
                        .UseLazyLoadingProxies()
                        .UseNpgsql("User ID=postgres;Host=localhost;Port=5432;Database=triple_triad"))
                .AddHostedService<MediatorHostedService>()
                .AddSignalR()
                .AddMessagePackProtocol()
                .Services
                .AddAppAuthentication()
                .AddMvc()
                .AddJsonOptions(x => x.SerializerSettings.Converters.Add(new StringEnumConverter()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .Services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "../TripleTriad.Client/dist";
                });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MainModule());
            builder.RegisterModule(new BackgroundTasksModule());
            builder.RegisterModule(new LogicModule());
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<SerilogMiddleware>();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/gameHub");
            });

            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../TripleTriad.Client";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
