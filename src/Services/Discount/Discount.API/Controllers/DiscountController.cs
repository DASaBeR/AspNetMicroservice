using Discount.API.Entities;
using Discount.API.Repositories;
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
		public DiscountController(IDiscountRepository repository , ILogger<DiscountController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		#region " ProductDiscount "

		[HttpGet("{productId}" , Name = "GetDiscount")]
		[ProducesResponseType(typeof(PDiscount), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<PDiscount>> GetDiscount(string productId)
		{
			var discount = await _repository.GetProductDiscount(productId);
			if (discount == null)
			{
				_logger.LogError($"Product with id {productId} dose not have discount.");
				return NoContent();
			}

			return Ok(discount);
		}

		[HttpPost]
		[ProducesResponseType(typeof(PDiscount), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> CreateDiscount([FromBody] PDiscount discount)
		{
			var resault = await _repository.CreateProductDiscount(discount);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with saving discount for product with id : {discount.Id}.");
				return BadRequest();
			}

			return CreatedAtRoute("GetDiscount", new { productId = discount.ProductId } , discount);
		}

		[HttpPut]
		[ProducesResponseType(typeof(PDiscount), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateDiscount([FromBody] PDiscount discount)
		{
			var resault = await _repository.UpdateProductDiscount(discount);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with saving changes for product with id : {discount.ProductId}.");
				return BadRequest();
			}

			return CreatedAtRoute("GetDiscount", new { productId = discount.ProductId } , discount);
		}

		[HttpDelete]
		[ProducesResponseType(typeof(PDiscount), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteDiscount([FromBody] PDiscount discount)
		{
			var resault = await _repository.DeleteProductDiscount(discount);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with deleting product with id : {discount.ProductId}.");
				return BadRequest();
			}

			return Ok();
		}

		#endregion


	}
}
