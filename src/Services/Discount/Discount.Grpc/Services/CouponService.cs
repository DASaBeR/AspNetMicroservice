using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Discount.Grpc.ViewModels;
using Discount.Grps.Entities;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
	public class CouponService : CouponProtoService.CouponProtoServiceBase
	{
		private readonly ICouponRepository _repository;
		private readonly IMapper _mapper;
		private readonly ILogger<CouponService> _logger;

		public CouponService(ICouponRepository repository, IMapper mapper, ILogger<CouponService> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public override async Task<CouponsResponse> GetCoupons(GetCouponsRequest request, ServerCallContext context)
		{
			var coupons = await _repository.GetCoupons();
			if (coupons == null)
			{
				throw new RpcException(new Status(StatusCode.NotFound, "There are no coupons yet."));
			}
			_logger.LogInformation($"{coupons.Count()} coupons found.");

			var couponsModel = _mapper.Map<IEnumerable<CouponModel>>(coupons);

			CouponsResponse response = new CouponsResponse();
			response.Coupons.AddRange(couponsModel);
			return response;
		}

		public override async Task<CouponModel> GetCoupon(GetCouponRequest request, ServerCallContext context)
		{
			var coupon = await _repository.GetCoupon(request.CouponCode);
			if (coupon == null)
			{
				throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with code : {request.CouponCode} dose not exist."));
			}
			_logger.LogInformation($"Coupon is retrieved with code : {coupon.Code} , Percent : {coupon.Percent}");

			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<CouponModel> CreateCoupon(CreateCouponRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<Coupon>(request.Coupon);
			var result = await _repository.CreateCoupon(coupon);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, $"Something went wrong with creating discount."));
			}
			_logger.LogInformation($"Coupon with code: {coupon.Code} , percentage: {coupon.Code} have been created.");

			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<CouponModel> UpdateCoupon(UpdateCouponRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<CouponVM>(request.Coupon);
			var result = await _repository.UpdateCoupon(coupon);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, $"Something went wrong with updating discount."));
			}
			_logger.LogInformation($"Coupon with Id: {coupon.Id} have been updated. Coupon Code : {coupon.Code}");

			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<DeleteCouponResponse> DeleteCoupon(DeleteCouponRequest request, ServerCallContext context)
		{
			var result = await _repository.DeleteCoupon(request.CouponCode);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with Code or Id = {request.CouponCode} dose not exist."));
			}
			_logger.LogInformation($"Coupon with Code or Id = {request.CouponCode} have been deleted.");

			DeleteCouponResponse response = new DeleteCouponResponse();
			response.Success = result;
			return response;
		}
	}
}
