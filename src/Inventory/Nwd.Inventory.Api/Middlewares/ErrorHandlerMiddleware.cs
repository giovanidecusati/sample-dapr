using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace Nwd.Inventory.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(FluentValidation.ValidationException), HandleFluentValidationException }
            };

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                HandleException(context);

                var response = context.Response;
                response.ContentType = "application/json";

                //switch (error)
                //{
                //    case AppException e:
                //        // custom application error
                //        response.StatusCode = (int)HttpStatusCode.BadRequest;
                //        break;
                //    case KeyNotFoundException e:
                //        // not found error
                //        response.StatusCode = (int)HttpStatusCode.NotFound;
                //        break;
                //    default:
                //        // unhandled error
                //        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        break;
                //}

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }

        private void HandleException(HttpContext context)
        {
            //var type = context.Exception.GetType();
            //if (_exceptionHandlers.ContainsKey(type))
            //{
            //    _exceptionHandlers[type].Invoke(context);
            //    return;
            //}

            //if (!context.Response.ModelState.IsValid)
            //{
            //    HandleInvalidModelStateException(context);
            //    return;
            //}
        }
        private void HandleFluentValidationException(ExceptionContext context)
        {
            var result = (FluentValidation.ValidationException)context.Exception;

            foreach (var error in result.Errors)
                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            var details = new ValidationProblemDetails(context.ModelState)
            {
                Instance = context.HttpContext.Request.Path,
                Detail = "Please refer to the errors property for additional details.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details)
            {
                ContentTypes = { "application/problem+json", }
            };

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Instance = context.HttpContext.Request.Path,
                Detail = "Please refer to the errors property for additional details.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details)
            {
                ContentTypes = { "application/problem+json", }
            };

            context.ExceptionHandled = true;
        }
    }
}
