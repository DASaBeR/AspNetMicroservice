using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
	public interface IDiscountRepository
	{

		#region " CRUD PDiscount "
		Task<PDiscount> GetProductDiscount(string productId);
		Task<bool> CreateProductDiscount(PDiscount discount);
		Task<bool> UpdateProductDiscount(PDiscount discount);
		Task<bool> DeleteProductDiscount(string productId);

		#endregion

	}
}
