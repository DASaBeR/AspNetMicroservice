using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Discount.Grpc.ViewModels;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		private readonly IDiscountRepository _repository;
		private readonly IMapper _mapper;
		private readonly ILogger<DiscountService> _logger;

		public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public override async Task<ProductDiscountModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var discount = await _repository.GetProductDiscount(request.ProductId);
			if (discount == null)
			{
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount for product with id : {request.ProductId} dose not exist."));
			}
			_logger.LogInformation($"Discount is retrieved for ProductId : {discount.ProductId} , Percent : {discount.Percent}");

			var discountModel = _mapper.Map<ProductDiscountModel>(discount);
			return discountModel;
		}

		public override async Task<ProductDiscountModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var discount = _mapper.Map<ProductDiscount>(request.ProductdDscount);
			var result = await _repository.CreateProductDiscount(discount);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, $"Something went wrong with creating discount."));
			}
			_logger.LogInformation($"Discount with Precent: {discount.Percent} for product with id : {discount.ProductId} have been created.");

			var discountModel = _mapper.Map<ProductDiscountModel>(discount);
			return discountModel;
		}

		public override async Task<ProductDiscountModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var discount = _mapper.Map<ProductDiscountVM>(request.ProductdDscount);
			var result = await _repository.UpdateProductDiscount(discount);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, $"Something went wrong with updating discount."));
			}
			_logger.LogInformation($"Discount for product with id : {discount.ProductId} have been updated.");

			var discountModel = _mapper.Map<ProductDiscountModel>(discount);
			return discountModel;
		}

		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var result = await _repository.DeleteProductDiscount(request.ProductId);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount for product with id : {request.ProductId} dose not exist."));
			}
			_logger.LogInformation($"Discount for product with id : {request.ProductId} have been deleted.");

			var response = new DeleteDiscountResponse
			{
				Success = result
			};
			return response;
		}
	}
}
