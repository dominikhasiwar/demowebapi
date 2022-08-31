using DemoWebApi.Exceptions;
using DemoWebApi.Models;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;

namespace DemoWebApi.Handlers
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Error(contextFeature.Error.Message, contextFeature.Error);

                        var errorModel = CreateErrorModel(contextFeature.Error);

                        context.Response.StatusCode = (int)errorModel.StatusCode;
                        await context.Response.WriteAsync(errorModel.ToString());
                    }
                });
            });
        }

        private static ErrorModel CreateErrorModel(Exception exception)
        {
            switch (exception)
            {
                case NotFoundException nex:
                    return new ErrorModel
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessage = nex.Message
                    };
                case ValidationException vex:
                    return new ValidationErrorModel
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "One or more validation errors occured!",
                        Errors = vex.Errors.Select(e => new ValidationError
                        {
                            ErrorCode = e.ErrorCode,
                            ErrorMessage = e.ErrorMessage,
                            PropertyName = e.PropertyName
                        }).ToArray()
                    };
                default:
                    return new ErrorModel
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrorMessage = exception.Message
                    };
            }
        }
    }
}
