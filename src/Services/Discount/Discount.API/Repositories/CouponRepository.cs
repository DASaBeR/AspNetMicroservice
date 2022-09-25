using Dapper;
using Discount.API.Entities;
using Discount.API.ViewModels;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
	public class CouponRepository : ICouponRepository
	{
		private readonly IConfiguration _configuration;
		public CouponRepository(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		#region " CRUD Coupon "

		public async Task<IEnumerable<CouponVM>> GetCoupons()
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var coupons = await connection
				.QueryAsync<CouponVM>("SELECT * FROM Coupon");

			return coupons;
		}
		public async Task<CouponVM> GetCoupon(string code)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var coupon = await connection
				.QueryFirstOrDefaultAsync<CouponVM>("SELECT * FROM Coupon WHERE Code = @Code", new { Code = code });

			return coupon;
		}

		public async Task<bool> CreateCoupon(Coupon coupon)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			var affected =
				await connection.ExecuteAsync
						("INSERT INTO Coupon (Id , Code , Description , Percent , CreateTimeStamp , IsExpired) VALUES (@Id , @Code , @Description , @Percent , @CreateTimeStamp , @IsExpired)",
								new {Id = coupon.Id , Code = coupon.Code , Description = coupon.Description, Percent = coupon.Percent, CreateTimeStamp = coupon.CreateTimeStamp , IsExpired = coupon.IsExpired });

			if (affected == 0)
				return false;

			return true;
		}

		public async Task<bool> UpdateCoupon(CouponVM coupon)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("UPDATE Coupon SET Code = @Code , Description = @Description , Percent = @Percent , IsExpired = @IsExpired WHERE Id = @Id",
								new { Code = coupon.Code, Description = coupon.Description, Percent = coupon.Percent, IsExpired = coupon.IsExpired, Id = coupon.Id });

			if (affected == 0)
				return false;

			return true;
		}


		public async Task<bool> DeleteCoupon(string code)
		{
			using var connection = new NpgsqlConnection
							(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

			var affected =
				await connection.ExecuteAsync
						("DELETE FROM Coupon WHERE Code = @Code",
								new { Code = code });

			if (affected == 0)
				return false;

			return true;
		}


		#endregion

	}
}
