using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Monke.Metrics.Exceptions
{
	public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			logger.LogWarning("An exception occurred while processing the request: {message}", exception.Message);
			httpContext.Response.StatusCode = exception switch
			{
				BadRequestException => StatusCodes.Status400BadRequest,
				NotFoundException => StatusCodes.Status404NotFound,
				_ => StatusCodes.Status500InternalServerError
			};

			return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
			{
				HttpContext = httpContext,
				Exception = exception,
				ProblemDetails = new ProblemDetails
				{
					Type = exception.GetType().Name,
					Title = "An error occured",
					Detail = exception.Message
				}
			});
		}
	}
}
