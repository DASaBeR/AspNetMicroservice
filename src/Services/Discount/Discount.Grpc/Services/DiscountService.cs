using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Discount.Grpc.ViewModels;
using Grpc.Core;
using Mapster;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		private readonly IDiscountRepository _repository;
		private readonly ILogger<DiscountService> _logger;

		public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public override async Task<ProductDiscountModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			try
			{
				var discount = await _repository.GetProductDiscount(request.ProductId);
				if (discount == null)
				{
					throw new RpcException(new Status(StatusCode.NotFound, $"Discount for product with id : {request.ProductId} dose not exist."));
				}
				_logger.LogInformation($"Discount is retrieved for ProductId : {discount.ProductId} , Percent : {discount.Percent}");

				var discountModel = discount.Adapt<ProductDiscountModel>();
				return discountModel;
			}
			catch (Exception e)
			{

				throw;
			}

		}

		public override async Task<ProductDiscountModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var discount = request.Adapt<ProductDiscount>();
			var result = await _repository.CreateProductDiscount(discount);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, $"Something went wrong with creating discount."));
			}
			_logger.LogInformation($"Discount with Precent: {discount.Percent} for product with id : {discount.ProductId} have been created.");

			var discountModel = discount.Adapt<ProductDiscountModel>();
			return discountModel;
		}

		public override async Task<ProductDiscountModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var discount = request.Adapt<ProductDiscountVM>();
			var result = await _repository.UpdateProductDiscount(discount);
			if (!result)
			{
				throw new RpcException(new Status(StatusCode.InvalidArgument, $"Something went wrong with updating discount."));
			}
			_logger.LogInformation($"Discount for product with id : {discount.ProductId} have been updated.");

			var discountModel = discount.Adapt<ProductDiscountModel>();
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
