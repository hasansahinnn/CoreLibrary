using Core.AutoFac;
using Core.SeriLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Core.CoreConfiguration.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Core.Application.Repository.EF;

namespace Core.CoreConfiguration
{
    public static class Configurations
    {
    
        public static void CoreServices<T>(this IServiceCollection services,IConfiguration Configuration) //  services.CoreServices<Startup>(Configuration);  -> Startup 
        {
            services.AddDbContext<DataContext>(options => options.EnableSensitiveDataLogging().UseSqlServer(Configuration.GetConnectionString("DataContext")));
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule(Configuration)
            });

            services.AddControllers().AddFluentValidation(s => // Actiondaki parametreleri otomatik kontrole eder.
            {
                s.RegisterValidatorsFromAssemblyContaining<T>();
                s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });
        }
        public static void CoreConfigure(this IApplicationBuilder app, IWebHostEnvironment env) // app.CoreConfigure(env);   -> Startup 
        {
            AutoFacManager.SetAutoFacContainer(app); // Autofac
            app.UseSerilogRequestLogging(); // Serilog

            app.AddGlobalErrorHandler()//serilog handler 
               .UseMySwagger()  // Swagger
               .UseAppCors(); // Cors

            app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(env.ContentRootPath, "Resources")),
            //    RequestPath = "/Resources"
            //});
        }
    }
}
