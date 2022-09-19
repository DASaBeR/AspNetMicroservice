using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
	public interface IDiscountRepository
	{

		#region " CRUD GCoupon "

		Task<IEnumerable<Coupon>> GetCoupons();
		Task<bool> CreateCoupon(Coupon coupon);
		Task<bool> UpdateCoupon(Coupon coupon);
		Task<bool> DeleteCoupon(Coupon coupon);

		#endregion

		#region " CRUD PDiscount "
		Task<PDiscount> GetProductDiscount(string productId);
		Task<bool> CreateProductDiscount(PDiscount discount);
		Task<bool> UpdateProductDiscount(PDiscount discount);
		Task<bool> DeleteProductDiscount(PDiscount discount);

		#endregion


	}
}
