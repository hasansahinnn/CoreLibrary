using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CoreConfiguration.Extensions
{
    public static class CorsExtension
    {
        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors("Cors");
            return app;
        }
        public static IServiceCollection UseAppCors(this IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("Cors", builder => {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                    builder.SetIsOriginAllowed(_ => true);
                });

            });
            return services;
        }
      
    }
}
