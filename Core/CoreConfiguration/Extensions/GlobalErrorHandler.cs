using Core.SeriLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace Core.CoreConfiguration.Extensions
{
    public static class GlobalErrorHandler
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder app)  // Microsoft.AspNetCore.Diagnostics package
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>(); // Catches Exception Detail
                ErrorLog error = new ErrorLog
                {
                    Path = exceptionHandlerPathFeature.Path,
                    StatusCode = context.Response.StatusCode,
                    Method = context.Request.Method,
                    ErrorMessage = exceptionHandlerPathFeature.Error.Message
                };

                context.Response.ContentType = "application/json";

                // --- Return ErrorLog
                var errorResponse = JsonConvert.SerializeObject(error);
                // Serilog
                Log.Error(errorResponse); 
                await context.Response.WriteAsync(errorResponse);
                // --- Return ErrorLog

                //var result = JsonConvert.SerializeObject(new { error = exceptionHandlerPathFeature.Error.Message });
                //await context.Response.WriteAsync(result);
            }));
            return app;
        }
    }
}
