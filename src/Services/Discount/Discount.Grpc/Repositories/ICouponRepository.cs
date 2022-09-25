using Discount.Grpc.ViewModels;
using Discount.Grps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
	public interface ICouponRepository
	{

		#region " CRUD GCoupon "

		Task<IEnumerable<CouponVM>> GetCoupons();
		Task<CouponVM> GetCoupon(string code);
		Task<bool> CreateCoupon(Coupon coupon);
		Task<bool> UpdateCoupon(Coupon coupon);
		Task<bool> DeleteCoupon(string code);

		#endregion

	}
}
