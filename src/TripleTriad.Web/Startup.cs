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
using TripleTriad.Requests.GuestPlayer;
using TripleTriad.Data;
using TripleTriad.Web.Filters;
using TripleTriad.Web.IoC;

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
                    => options.UseNpgsql("User ID=postgres;Host=localhost;Port=5432;Database=triple_triad"))
                .AddSession(options =>
                {
                    options.Cookie.Name = ".TripleTriad.Session";
                })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<EnsurePlayerIdExistsActionFilter>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();

            app.UseMvc();
        }
    }
}
