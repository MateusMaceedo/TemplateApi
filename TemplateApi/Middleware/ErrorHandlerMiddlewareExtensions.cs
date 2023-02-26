using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

                    if (error != null)
                    {
                        var errorMessage = new ErrorMessage
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "An unexpected error occurred.",
                            Details = error.Error.Message
                        };

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
                    }
                });
            });
        }
    }
}
