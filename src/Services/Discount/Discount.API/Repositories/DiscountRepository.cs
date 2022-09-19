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


		#region " CRUD Coupon "

		public async Task<IEnumerable<Coupon>> GetCoupons()
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var coupons = await connection
				.QueryAsync<Coupon>("SELECT * FROM Coupon");

			return coupons;
		}

		public async Task<bool> CreateCoupon(Coupon coupon)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("INSERT INTO Coupon (Description , Amount , IsExpired) VALUES (@Description , @Amount , @IsExpired)",
								new { Description = coupon.Description, Amount = coupon.Amount, IsExpired = coupon.IsExpired });

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> UpdateCoupon(Coupon coupon)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("UPDATE Coupon SET Code = @Code , Description = @Description , Amount = @Amount , IsExpired = @IsExpired WHERE Id = @Id",
								new {Code = coupon.Code, Description = coupon.Description, Amount = coupon.Amount, IsExpired = coupon.IsExpired, Id = coupon.Id });

			if (affected == 0)
				return false;

			return true;
		}


		public async Task<bool> DeleteCoupon(Coupon coupon)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("DELETE FROM Coupon WHERE Code = @Code",
								new { Code = coupon.Code });

			if (affected == 0)
				return false;

			return true;
		}


		#endregion


		#region " CRUD ProductDiscount "

		public async Task<PDiscount> GetProductDiscount(string productId)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var discount = await connection
				.QueryFirstOrDefaultAsync<PDiscount>("SELECT * FROM ProductDiscount WHERE ProductId = @ProductId" , new { ProductId = productId });

			return discount;
		}

		public async Task<bool> CreateProductDiscount(PDiscount discount)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("INSERT INTO ProductDiscount (ProductId , Description , Amount , IsExpired) VALUES (@ProductId , @Description , @Amount , @IsExpired)",
								new { ProductId = discount.ProductId , Description = discount.Description , Amount = discount.Amount , IsExpired = discount.IsExpired });

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
						("UPDATE ProductDiscount SET ProductId = @ProductId , Description = @Description , Amount = @Amount , IsExpired = @IsExpired WHERE Id = @Id",
								new { ProductId = discount.ProductId, Description = discount.Description, Amount = discount.Amount, IsExpired = discount.IsExpired, Id = discount.Id });

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> DeleteProductDiscount(PDiscount discount)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("DELETE FROM Coupon WHERE Code = @Code",
								new { ProductId = discount.ProductId });

			if (affected == 0)
				return false;

			return true;
		}

		#endregion

	}
}
