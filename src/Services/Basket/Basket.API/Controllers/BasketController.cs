using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _repository;
		private readonly DiscountGrpcService _discountGrpcServices;
		private readonly CouponGrpcService _couponGrpcService;
		private readonly ILogger<BasketController> _logger;
		public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, CouponGrpcService couponGrpcService, ILogger<BasketController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_discountGrpcServices = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
			_couponGrpcService = couponGrpcService ?? throw new ArgumentNullException(nameof(couponGrpcService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		[HttpGet("{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			var basket = await _repository.GetBasket(userName);
			return Ok(basket ?? new ShoppingCart(userName)); //if basket was null, new one basket for user.
		}

		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
		{
			// TODO : Communicate with Discount.Grpc
			// and Calculate latest prices of product into shopping cart
			// consume Discount Grpc
			foreach (var item in basket.Items)
			{
				var discount = await _discountGrpcServices.GetDiscount(item.ProductId);
				if (!discount.IsExpired)
				{
					item.Price = item.Price - (item.Price * Convert.ToDecimal(discount.Percent / 100));
				}
			}

			return Ok(await _repository.UpdateBasket(basket));
		}

		[HttpDelete("{userName}", Name = "DeleteBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteBasket(string userName)
		{
			await _repository.DeleteBasket(userName);
			return Ok();
		}

		[HttpPost("{userName}", Name = "CouponDiscount")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> CouponDiscount(string userName , [FromBody] string couponCode)
		{

			var basket = await _repository.GetBasket(userName);
			// TODO : Communicate with Discount.Grpc
			// and Calculate latest prices of product into shopping cart
			// consume Discount Grpc
			foreach (var item in basket.Items)
			{
				var coupon = await _couponGrpcService.GetCouponDiscount(couponCode);
				if (!coupon.IsExpired)
				{
					item.Price = item.Price - (item.Price * Convert.ToDecimal(coupon.Percent / 100));
				}
			}

			return Ok(await _repository.UpdateBasket(basket));
		}

	}
}
