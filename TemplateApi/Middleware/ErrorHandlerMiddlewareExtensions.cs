using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using TemplateApi.Application.Rest;

namespace TemplateApi.Middleware
{
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    var errorMessages = new List<ErrorMessage>();
                    var exception = error.Error;
                    while (exception != null)
                    {
                        var errorMessage = new ErrorMessage
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = exception.Message,
                            Details = exception.StackTrace
                        };
                        errorMessages.Add(errorMessage);
                        exception = exception.InnerException;
                    }

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessages));
                });
            });
        }
    }
}
