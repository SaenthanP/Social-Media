using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalNetworkService.MessageServiceConsumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AutoMapper;
using PersonalNetworkService.Events;
using Neo4j.Driver;
using PersonalNetworkService.Services;
using PersonalNetworkService.MessageServicePublisher;

namespace PersonalNetworkService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHostedService<MessagingClient>();
            services.AddSingleton(GraphDatabase.Driver(Configuration.GetSection("Neo4jHost").Value, AuthTokens.Basic(Configuration.GetSection("Neo4jUsername").Value, Configuration.GetSection("Neo4jPassword").Value)));
            services.AddSingleton<IEventProcessing,EventProcessing>();
            services.AddSingleton<IUserNetworkService,UserNetworkService>();
            services.AddSingleton<IMessageClient,MessageClient>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PersonalNetworkService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonalNetworkService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
