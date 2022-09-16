using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
	public class CatalogContext : ICatalogContext
	{
		public CatalogContext(IConfiguration configuration)
		{
			var client = new MongoClient(configuration.GetSection("DatabaseSettings:ConnectionString").ToString());
			var database = client.GetDatabase(configuration.GetSection("DatabaseSettings:DatabaseName").ToString());

			Products = database.GetCollection<Product>(configuration.GetSection("DatabaseSettings:CollectionName").ToString());
			//CatalogContextSeed.SeedData(Product);
		}

		public IMongoCollection<Product> Products { get; }
	}
}
