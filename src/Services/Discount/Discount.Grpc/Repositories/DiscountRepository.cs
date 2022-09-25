using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.ViewModels;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{
		private readonly IConfiguration _configuration;
		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}


		#region " CRUD ProductDiscount "

		public async Task<ProductDiscountVM> GetProductDiscount(string productId)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var discount = await connection
				.QueryFirstOrDefaultAsync<ProductDiscountVM>("SELECT * FROM ProductDiscount WHERE ProductId = @ProductId" , new { ProductId = productId });

			if (discount == null)
				return new ProductDiscountVM
				{ ProductId = "No Discount", Percent = 0, Description = "There is No Discount for This Product." };
			return discount;
		}

		public async Task<bool> CreateProductDiscount(ProductDiscount discount)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("INSERT INTO ProductDiscount (Id , ProductId , Description , Percent , CreateTimeStamp , IsExpired) VALUES (@Id , @ProductId , @Description , @Percent , @CreateTimeStamp , @IsExpired)",
								new {Id = discount.Id , ProductId = discount.ProductId , Description = discount.Description, Percent = discount.Percent, CreateTimeStamp = discount.CreateTimeStamp, IsExpired = discount.IsExpired });

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> UpdateProductDiscount(ProductDiscountVM discount)
		{
			using var connection = new NpgsqlConnection
										(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("UPDATE ProductDiscount SET Description = @Description , Percent = @Percent , IsExpired = @IsExpired WHERE ProductId = @ProductId",
								new {Description = discount.Description, Percent = discount.Percent, IsExpired = discount.IsExpired, ProductId = discount.ProductId });

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
