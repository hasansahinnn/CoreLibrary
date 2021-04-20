using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Caching;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.AutoFac
{
    public static  class AutoFacManager
    {

        private static IServiceProvider containerProvider;
        public static void SetAutoFacContainer(this IApplicationBuilder app)
        {
            containerProvider = app.ApplicationServices;
        }
        public static T Resolve<T>()
        {
            return (T)containerProvider.GetService(typeof(T));
        }
    }
}
