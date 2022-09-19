using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
	interface IDiscountRepository
	{

		#region " CRUD GCoupon "

		Task<IEnumerable<Coupon>> GetCoupons();
		Task<bool> CreateCoupon(Coupon coupon);
		Task<bool> UpdateCoupon(Coupon coupon);
		Task<bool> DeleteCoupon(Coupon coupon);

		#endregion

		#region " CRUD PDiscount "
		Task<PDiscount> GetProductDiscount(string productId);
		Task<bool> CreateProductDiscount(PDiscount pDiscount);
		Task<bool> UpdateProductDiscount(PDiscount pDiscount);
		Task<bool> DeleteProductDiscount(PDiscount pDiscount);

		#endregion


	}
}
