using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Core.Application;
using Core.Application.Interfaces;
using Core.Application.MailSender;
using Core.Caching;
using Core.DataSecurity;
using Core.Intercepters;
using Core.Models;
using Core.Validation;
using Microsoft.EntityFrameworkCore.DataEncryption;
using System.Linq;

namespace Core
{

    public class AutoFacConfiguration : Module
    {
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationSettings>().As<IApplicationSettings>().SingleInstance();
            builder.RegisterType<TestDataModelValidator>().As<ITestModelValidator>().SingleInstance();
            builder.RegisterType<Mailer>().As<IMailer>().SingleInstance();
            builder.RegisterType<Encryption>().As<IEncryptionProvider>().SingleInstance();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();

            builder.RegisterType<DataModel>().As<IDataModel>().SingleInstance();
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load(nameof(Core))) // Eğer Dizinde İlgili Interface olmazsa hata verir.
                .Where(x => x.Namespace.Contains("Core.Models")).As(x => x.GetInterfaces().FirstOrDefault(y => y.Name == "I" + x.Name)).SingleInstance();
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
