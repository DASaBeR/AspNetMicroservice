﻿using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours
{
	public class ValidationBehaviour<TRequest, TResponse>
		: IPipelineBehavior<TRequest, TResponse>
		where TRequest : MediatR.IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _valdaitors;
		public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			_valdaitors = validators ?? throw new ArgumentNullException(nameof(validators));
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (_valdaitors.Any())
			{
				var context = new ValidationContext<TRequest>(request);

				var validationResults = await Task.WhenAll(_valdaitors.Select(v => v.ValidateAsync(context, cancellationToken)));
				var failures = validationResults.SelectMany(e => e.Errors).Where(f => f != null).ToList();

				if (failures.Count != 0)
				{
					throw new ValidationException(failures);
				}
			}

			return await next();
		}
	} 
}
