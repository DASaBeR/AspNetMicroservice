﻿using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Behaviours
{
	public class UnhandledExceptionBehaviour<TRequest, TResponse>
		: IPipelineBehavior<TRequest, TResponse>
		where TRequest : MediatR.IRequest<TResponse>
	{
		private readonly ILogger<TRequest> _logger;
		public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;
				_logger.LogError(ex, "Application Request : Unhandled Exception for Request {Name} {@Request}" , requestName, request);
				throw;
			}
		}
	}
}
