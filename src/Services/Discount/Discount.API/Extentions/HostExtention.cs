using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Extentions
{
	public static class HostExtention
	{
		public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
		{
			int retryForAvailability = retry.Value;

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var configuration = services.GetRequiredService<IConfiguration>();
				var logger = services.GetRequiredService<ILogger<TContext>>();

				try
				{
					logger.LogInformation("Migration postgresql database started.");
					using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
					connection.Open();

					using var command = new NpgsqlCommand
					{
						Connection = connection
					};
					command.CommandText = "DROP TABLE IF EXISTS ProductDiscount,Coupon";
					command.ExecuteNonQuery();

					command.CommandText = @"CREATE TABLE Coupon(ID UUID PRIMARY KEY DEFAULT gen_random_uuid()	NOT NULL,
																											Code     Text NOT NULL UNIQUE,
																											Description     TEXT,
																											Percent          DOUBLE PRECISION DEFAULT 0,
																											CreateTimeStamp		TEXT,
																											IsExpired		BOOL);";
					command.ExecuteNonQuery();

					command.CommandText = @"CREATE TABLE ProductDiscount(ID UUID PRIMARY KEY	DEFAULT gen_random_uuid()	NOT NULL,
																															ProductId     VARCHAR(24) NOT NULL UNIQUE,
																															Description     TEXT,
																															Percent          DOUBLE PRECISION DEFAULT 0,
																															CreateTimeStamp		TEXT,
																															IsExpired		BOOL);";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO Coupon (Code, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('Christmas2021', 'Christmas 2021 Offer', 20, '1632134746' , True);";
					command.ExecuteNonQuery();
					command.CommandText = "INSERT INTO Coupon (Code, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('Summer2022', 'Summer 2022 Offer', 10, '1663677917' , False);";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO ProductDiscount (ProductId, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('602d2149e773f2a3990b47f5', 'IPhone Discount', 5, '1632114146' , True);";
					command.ExecuteNonQuery();
					command.CommandText = "INSERT INTO ProductDiscount (ProductId, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('602d2149e773f2a3990b47f7', 'Huawie Discount', 15, '1663677817' , False);";
					command.ExecuteNonQuery();

					logger.LogInformation("Migration postgresql database done.");

				}
				catch (NpgsqlException ex)
				{

					logger.LogError(ex , "An error occured while migrating the postresql database.");

					if (retryForAvailability <50)
					{
						retryForAvailability++;
						System.Threading.Thread.Sleep(2000);
						MigrateDatabase<TContext>(host, retryForAvailability);
					}
				}

				return host;

			}
		}
	}
}
