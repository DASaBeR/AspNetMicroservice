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
	public class CouponController : ControllerBase
	{
		private readonly ICouponRepository _repository;
		private readonly ILogger<CouponController> _logger;
		public CouponController(ICouponRepository repository, ILogger<CouponController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		#region " Coupon "

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Coupon>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Coupon>>> GetCoupons()
		{
			var coupons = await _repository.GetCoupons();
			if (coupons == null)
			{
				_logger.LogInformation($"There is No Coupons.");
				return NoContent();
			}

			return Ok(coupons);
		}

		[HttpGet("{code}", Name = "GetCoupon")]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> GetCoupon(string code)
		{
			var coupon = await _repository.GetCoupon(code);
			if (coupon == null)
			{
				_logger.LogInformation($"There is No Coupon with Code : {code} .");
				return NotFound();
			}
			return Ok(coupon);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> CreateCoupon([FromBody] Coupon coupon)
		{
			var resault = await _repository.CreateCoupon(coupon);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with saving Coupon with Code : {coupon.Code}.");
				return BadRequest();
			}

			return CreatedAtRoute("GetCoupon", new { code = coupon.Code }, coupon);
		}

		[HttpPut]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateCoupon([FromBody] Coupon coupon)
		{
			var resault = await _repository.UpdateCoupon(coupon);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with saving changes for coupon with Code : {coupon.Code}.");
				return BadRequest();
			}

			return CreatedAtRoute("GetCoupon", new { code = coupon.Code }, coupon);
		}

		[HttpDelete("{code}", Name = "DeleteCoupon")]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteCoupon(string code)
		{
			var resault = await _repository.DeleteCoupon(code);
			if (!resault)
			{
				_logger.LogError($"Something went wrong with deleting product with Code : {code}.");
				return NotFound();
			}

			return Ok();
		}

		#endregion

	}
}
