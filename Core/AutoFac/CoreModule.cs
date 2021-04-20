using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Core.Application.Interfaces;
using Core.Application.MailSender;
using Core.Application.Repository.EF;
using Core.Caching;
using Core.CoreConfiguration.Extensions;
using Core.DataSecurity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.AutoFac
{
    public class CoreModule : ICoreModule
    {
        public CoreModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void Load(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.EnableSensitiveDataLogging().UseSqlServer(Configuration.GetConnectionString("DataContext")));
            services.AddResponseCaching(); // Caching
            services.AddMemoryCache(); // Caching
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Stopwatch>();
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            ServiceTool.Create(services);
            services.UseMySwagger()
                .UseAppCors()
                .UseJwt(Configuration);


            //services.AddResponseCaching(); // Caching  -> Add Startup
    

           
        }
    }
}
