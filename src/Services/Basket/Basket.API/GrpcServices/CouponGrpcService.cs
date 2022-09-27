using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
	public class CouponGrpcService
	{
		private readonly CouponProtoService.CouponProtoServiceClient _couponProtoService;

		public CouponGrpcService(CouponProtoService.CouponProtoServiceClient couponProtoService)
		{
			_couponProtoService = couponProtoService ?? throw new ArgumentNullException(nameof(couponProtoService));
		}

		public async Task<CouponModel> GetCouponDiscount(string couponCode)
		{
			var couponRequest = new GetCouponRequest { CouponCode = couponCode };

			return await _couponProtoService.GetCouponAsync(couponRequest);
		}
	}
}
