using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Extentions
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

					command.CommandText = @"CREATE TABLE Coupon(ID UUID PRIMARY KEY		NOT NULL,
																											Code     Text NOT NULL UNIQUE,
																											Description     TEXT,
																											Percent          DOUBLE PRECISION DEFAULT 0,
																											CreateTimeStamp		TEXT,
																											IsExpired		BOOL	DEFAULT FALSE);";
					command.ExecuteNonQuery();

					command.CommandText = @"CREATE TABLE ProductDiscount(ID UUID PRIMARY KEY	NOT NULL,
																															ProductId     VARCHAR(24) NOT NULL UNIQUE,
																															Description     TEXT,
																															Percent          DOUBLE PRECISION DEFAULT 0,
																															CreateTimeStamp		TEXT,
																															IsExpired		BOOL	DEFAULT FALSE);";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO Coupon (Id , Code, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('26b0f4e9-cd2a-4646-97a2-9fb7643d7100' , 'Christmas2021', 'Christmas 2021 Offer', 20, '1632134746' , True);";
					command.ExecuteNonQuery();
					command.CommandText = "INSERT INTO Coupon (Id , Code, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('bc25154e-f837-417f-91fe-c77e93b2de18' , 'Summer2022', 'Summer 2022 Offer', 10, '1663677917' , False);";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO ProductDiscount (Id , ProductId, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('af72b536-fae7-4edc-9fab-f2af74f9b648' , '602d2149e773f2a3990b47f5', 'IPhone Discount', 5, '1632114146' , True);";
					command.ExecuteNonQuery();
					command.CommandText = "INSERT INTO ProductDiscount (Id , ProductId, Description, Percent, CreateTimeStamp , IsExpired) VALUES ('4af6dd6f-0306-40f2-a11f-e7efbf70a300' , '602d2149e773f2a3990b47f7', 'Huawie Discount', 15, '1663677817' , False);";
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
