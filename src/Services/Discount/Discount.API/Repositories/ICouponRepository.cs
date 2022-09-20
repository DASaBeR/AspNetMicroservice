using Discount.API.Entities;
using Discount.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
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
