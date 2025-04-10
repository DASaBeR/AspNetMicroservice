﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
	public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
	{
		public DeleteOrderCommandValidator()
		{
			RuleFor(p => p.Id).NotEmpty().WithMessage("{Id} is required.");

			RuleFor(p => p.UserName)
				.NotEmpty().WithMessage("{UserName} is required.")
				.NotNull()
				.MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

			RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is required.");

			RuleFor(p => p.TotalPrice).NotEmpty().WithMessage("{TotalPrice} is required.")
				.GreaterThan(0).WithMessage("{TotalPrice} must be greater than zero.");
		}
	}
}
