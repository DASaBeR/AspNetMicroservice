using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure;
using System;

namespace Ordering.API
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

			//MassTransit-RabbitMQ Configuration
			services.AddMassTransit(config =>
			{
				config.AddConsumer<BasketCheckoutConsumer>();

				config.UsingRabbitMq((context, configurator) =>
				{
					configurator.Host(new Uri(Configuration["MessageBroker:Host"]!), host =>
					{
						host.Username(Configuration["MessageBroker:UserName"]);
						host.Password(Configuration["MessageBroker:Password"]);
					});
					configurator.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
					{
						c.ConfigureConsumer<BasketCheckoutConsumer>(context);
					});

				});
			});

			services.AddAutoMapper(typeof(Startup));
			services.AddScoped<BasketCheckoutConsumer>();

			services.AddApplicationServices();
			services.AddInfrastructureServices(Configuration);

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
