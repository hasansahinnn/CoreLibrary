using System;
using System.Collections.Generic;
using System.Text;

namespace Core.JwtSecurity
{
    public static class JwtConfigProvider
    {
        public static JwtSettingsConfig _config;
        public static void SetConfig(JwtSettingsConfig config) => _config = config;
    }
    public class JwtSettingsConfig
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }



    /* 
     
          "JwtOptions": {
            "Audience": "emoda",
            "Issuer": "emoda",
            "AccessTokenExpiration": 365,
            "SecurityKey": "mysupersecretkeymysupersecretkey"
          },
     
     
    */
}
