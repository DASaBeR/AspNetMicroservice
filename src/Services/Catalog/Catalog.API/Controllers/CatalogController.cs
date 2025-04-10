﻿using Catalog.API.Entities;
using Catalog.API.Repositories;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		private readonly IProductRepository _repository;
		private readonly ILogger<CatalogController> _logger;
		public CatalogController(IProductRepository repository , ILogger<CatalogController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		#region " Queries "
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _repository.GetProducts();
			return Ok(products);
		}

		[HttpGet("{id:length(24)}", Name = "GetProduct")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> GetProduct(string id)
		{
			var product = await _repository.GetProduct(id);
			if (product == null)
			{
				_logger.LogError($"Product with id {id}, not found.");
				return NotFound();
			}
			return Ok(product);
		}

		[Route("[action]/{name}", Name = "GetProductByName")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
		{
			var products = await _repository.GetProductByName(name);
			if (products == null)
			{
				_logger.LogError($"Products with name {name}, not found.");
				return NotFound();
			}

			return Ok(products);
		}

		[Route("[action]/{category}", Name = "GetProductByCategory")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
		{
			var products = await _repository.GetProductByCategory(category);
			if (products == null)
			{
				_logger.LogError($"Products with category {category}, not found.");
				return NotFound();
			}

			return Ok(products);
		}
		#endregion

		#region " CRUD "
		[HttpPost]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
		{
			await _repository.CreateProduct(product);

			return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
		}

		[HttpPut]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateProduct([FromBody] Product product)
		{
			var result = await _repository.UpdateProduct(product);
			if (!result)
			{
				_logger.LogError($"Product with id {product.Id} did not update.");
				return BadRequest();
			}
			return Ok();
		}

		[HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			var result = await _repository.DeleteProduct(id);
			if (!result)
			{
				_logger.LogError($"Product with id {id}, not found.");
				return NotFound();
			}
			return Ok();
		}
		#endregion

	}
}
