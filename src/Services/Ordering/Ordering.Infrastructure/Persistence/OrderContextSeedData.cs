using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
	public class OrderContextSeedData
	{
		public static async Task SeedAsync(OrderContext orderContext , ILogger<OrderContextSeedData> logger)
		{
			if (!orderContext.Orders.Any())
			{
				orderContext.Orders.AddRange(GetPreconfiguredOrders());
				await orderContext.SaveChangesAsync();
				logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
			}
		}

		private static IEnumerable<Order> GetPreconfiguredOrders()
		{
			return new List<Order>
			{
				new Order() {UserName = "SaBeR" , FirstName = "Mohsen" , LastName = "Saberi" , EmailAddress = "m.saberi3d@gmail.com" , AddressLine = "AhmadAbad" , Country = "Iran" , State = "KhorasanRazavi" , City = "Mashhad" , TotalPrice = 350000}
			};
		}

	}
}
