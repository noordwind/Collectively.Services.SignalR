using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using Collectively.Services.SignalR.Hubs;
using Collectively.Common.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Collectively.Common.RabbitMq;
using Collectively.Common.Security;
using Collectively.Messages.Events;
using Collectively.Messages.Commands;
using Collectively.Common.Extensions;
using RawRabbit.Configuration;
using System.Reflection;
using Collectively.Common.Services;
using Collectively.Common.ServiceClients;
using Collectively.Common.Exceptionless;
using Newtonsoft.Json;
using Collectively.Services.SignalR.Framework.IoC;
using Collectively.Services.SignalR.Services;

namespace Collectively.Services.SignalR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IContainer applicationContainer, IConfiguration configuration) 
        {
            this.ApplicationContainer = applicationContainer;
                this.Configuration = configuration;
               
        }
        public IContainer ApplicationContainer { get; private set; }
        public static ILifetimeScope LifetimeScope { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog(Configuration);
            services.AddWebEncoders();
            services.AddCors();
            services.AddMvc()
                    .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);;
            services.AddSignalR();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerLifetimeScope();
            builder.RegisterInstance(Configuration.GetSettings<ExceptionlessSettings>()).SingleInstance();
            builder.RegisterType<ExceptionlessExceptionHandler>().As<IExceptionHandler>().SingleInstance();
            builder.RegisterType<Handler>().As<IHandler>();
            builder.RegisterType<HubService>().As<IHubService>();
            builder.RegisterModule<ServiceClientModule>();
            builder.RegisterModule<ServiceClientsModule>();
            SecurityContainer.Register(builder, Configuration);
            RabbitMqContainer.Register(builder, Configuration.GetSettings<RawRabbitConfiguration>());
            ApplicationContainer = builder.Build();
            LifetimeScope = ApplicationContainer.BeginLifetimeScope();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            app.UseSerilog(loggerFactory);
            app.UseCors(builder => builder.AllowAnyHeader()
	            .AllowAnyMethod()
	            .AllowAnyOrigin()
	            .AllowCredentials());
            app.UseSignalR(routes =>
            {
                routes.MapHub<CollectivelyHub>("collectively");
            });
            app.UseMvc();
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
