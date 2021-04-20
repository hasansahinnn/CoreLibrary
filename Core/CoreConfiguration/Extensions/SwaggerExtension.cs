using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Core.CoreConfiguration.Extensions
{
    public static class SwaggerExtension
    {
        public static IApplicationBuilder UseMySwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", " Core Api Version 1");
                c.RoutePrefix = "swagger";

            });
            return app;
        }
        public static IServiceCollection UseMySwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Web Core Api",
                    Description = "Created By Emoda Yazilim",

                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Emoda Yazilim",
                        Email = "info@emodayazilim.com",
                        Url = new Uri("http://www.emodayazilim.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Tüm Hakları Saklıdır",

                    }
                });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {

                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Bearer yazdıktan sonra bir boşluk bırakıp ardından token girmeniz gerekmektedir",
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

            });
            return services;
        }

    }
}
