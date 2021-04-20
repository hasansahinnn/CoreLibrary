using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application
{

    public class ApplicationSettings : IApplicationSettings
    {
        public IConfiguration Configuration { get; }
        public ApplicationSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string Version { get { return Configuration.GetSection("ApplicationSettings:Version").Value; } }
    }



    /* 
     
     
      "ApplicationSettings":{
        "Version": "1.0",
      }
     
     
     */
}
