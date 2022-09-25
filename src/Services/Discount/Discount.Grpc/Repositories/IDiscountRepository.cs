using Discount.Grpc.Entities;
using Discount.Grpc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
	public interface IDiscountRepository
	{

		#region " CRUD PDiscount "
		Task<ProductDiscountVM> GetProductDiscount(string productId);
		Task<bool> CreateProductDiscount(ProductDiscount discount);
		Task<bool> UpdateProductDiscount(ProductDiscountVM discount);
		Task<bool> DeleteProductDiscount(string productId);

		#endregion

	}
}
