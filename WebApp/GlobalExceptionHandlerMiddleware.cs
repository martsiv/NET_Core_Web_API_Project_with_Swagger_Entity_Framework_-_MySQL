﻿using ApplicationCore.CustomException;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace WebApp
{
	public class GlobalExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public GlobalExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (HttpException httpError)
			{
				await CreateResponse(context, httpError.Status, httpError.Message);
			}
			catch (ValidationException validationError)
			{
				await CreateResponse(context, HttpStatusCode.BadRequest, validationError.Message);
			}
			catch (KeyNotFoundException error)
			{
				await CreateResponse(context, HttpStatusCode.NotFound, error.Message);
			}
			catch (Exception error)
			{
				await CreateResponse(context, HttpStatusCode.InternalServerError, error?.Message);
			}
		}

		private async Task CreateResponse(HttpContext context,
									HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
									string message = "Unknown error type!")
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;
			var result = JsonSerializer.Serialize(new { message });
			await context.Response.WriteAsync(result);
		}
	}
}
