using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IMediator _mediator;

		public OrderController(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		[HttpGet("{userName}", Name = "GetOrdersByUserName")]
		[ProducesResponseType(typeof(IEnumerable<OrdersVM>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<OrdersVM>>> GetOrdersByUserName(string userName)
		{
			var query = new GetOrdersListQuery(userName);
			var orders = await _mediator.Send(query);
			return Ok(orders);
		}

		#region " CRUD "

		[HttpPost(Name = "CheckoutOrder")]
		[ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Guid>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpPut(Name = "UpdateOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
		{
			await _mediator.Send(command);
			return NoContent();
		}

		[HttpDelete("{id}", Name = "DeleteOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> DeleteOrder(Guid id)
		{
			var command = new DeleteOrderCommand() { Id = id };
			await _mediator.Send(command);
			return NoContent();
		}

		
		#endregion

	}
}
