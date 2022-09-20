using Discount.API.Entities;
using Discount.API.Repositories;
using Discount.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository _repository;
		private readonly ILogger<DiscountController> _logger;
		public DiscountController(IDiscountRepository repository, ILogger<DiscountController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		#region " CRUD ProductDiscount "

		[HttpGet("{productId}", Name = "GetDiscount")]
		[ProducesResponseType(typeof(ProductDiscountVM), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ProductDiscountVM>> GetDiscount(string productId)
		{
			var discount = await _repository.GetProductDiscount(productId);
			if (discount == null)
			{
				_logger.LogError($"Product with id {productId} dose not have discount.");
				return NotFound();
			}

			return Ok(discount);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ProductDiscount), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> CreateDiscount([FromBody] ProductDiscount discount)
		{
			var resault = await _repository.CreateProductDiscount(discount);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with saving discount for product with id : {discount.ProductId}.");
				return BadRequest();
			}

			return CreatedAtRoute("GetDiscount", new { productId = discount.ProductId }, discount);
		}

		[HttpPut]
		[ProducesResponseType(typeof(ProductDiscountVM), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateDiscount([FromBody] ProductDiscountVM discount)
		{
			var resault = await _repository.UpdateProductDiscount(discount);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with saving changes for product with id : {discount.ProductId}.");
				return BadRequest();
			}

			return RedirectToAction("GetDiscount" , new { productId = discount.ProductId });
		}

		[HttpDelete("{productId}", Name = "DeleteDiscount")]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteDiscount(string productId)
		{
			var resault = await _repository.DeleteProductDiscount(productId);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with deleting product with id : {productId}.");
				return NotFound();
			}

			return Ok();
		}

		#endregion


	}
}
