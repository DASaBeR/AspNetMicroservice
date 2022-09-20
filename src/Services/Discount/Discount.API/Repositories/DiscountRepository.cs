using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{
		private readonly IConfiguration _configuration;
		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}


		#region " CRUD ProductDiscount "

		public async Task<PDiscount> GetProductDiscount(string productId)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var discount = await connection
				.QueryFirstOrDefaultAsync<PDiscount>("SELECT * FROM ProductDiscount WHERE ProductId = @ProductId" , new { ProductId = productId });

			if (discount == null)
				return new PDiscount
				{ ProductId = "No Discount", Percent = 0, Description = "There is No Discount for This Product." };
			return discount;
		}

		public async Task<bool> CreateProductDiscount(PDiscount discount)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("INSERT INTO ProductDiscount (ProductId , Description , Percent , IsExpired) VALUES (@ProductId , @Description , @Percent , @IsExpired)",
								new { ProductId = discount.ProductId , Description = discount.Description , Percent = discount.Percent , IsExpired = discount.IsExpired });

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> UpdateProductDiscount(PDiscount discount)
		{
			using var connection = new NpgsqlConnection
										(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("UPDATE ProductDiscount SET ProductId = @ProductId , Description = @Description , Percent = @Percent , IsExpired = @IsExpired WHERE Id = @Id",
								new { ProductId = discount.ProductId, Description = discount.Description, Percent = discount.Percent, IsExpired = discount.IsExpired, Id = discount.Id });

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> DeleteProductDiscount(string productId)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("DELETE FROM ProductDiscount WHERE ProductId = @ProductId",
								new { ProductId = productId });

			if (affected == 0)
				return false;

			return true;
		}

		#endregion

	}
}
