﻿using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
	public class DiscountGrpcService
	{
		private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
		{
			_discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
		}

		public async Task<ProductDiscountModel> GetDiscount(string productId)
		{
			var discountRequest = new GetDiscountRequest { ProductId = productId };

			return await _discountProtoService.GetDiscountAsync(discountRequest);
		}

	}
}
