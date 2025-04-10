﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Extentions
{
	public static class HostExtentions
	{
		public static IHost MigrateDatabase<TContext>(this IHost host,
																				Action<TContext, IServiceProvider> seeder,
																				int? retry = 0) where TContext : DbContext
		{
			int retryForAvailability = retry.Value;

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var logger = services.GetRequiredService<ILogger<TContext>>();
				var context = services.GetService<TContext>();

				try
				{
					logger.LogInformation("Migration database associated with context {DbContextName}", typeof(TContext).Name);

					InvokeSeeder(seeder, context, services);

					logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
				}
				catch (SqlException ex)
				{
					logger.LogError(ex, "An error occurred while migarting database used context {DbContextName}", typeof(TContext).Name);

					if (retryForAvailability < 50)
					{
						retryForAvailability++;
						System.Threading.Thread.Sleep(2000);
						MigrateDatabase<TContext>(host, seeder, retryForAvailability);
					}
				}
				return host;
			}
		}

		private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
																							TContext context,
																							IServiceProvider services)
																							where TContext : DbContext
		{
			context.Database.Migrate();
			seeder(context, services);
		}
	}
}
