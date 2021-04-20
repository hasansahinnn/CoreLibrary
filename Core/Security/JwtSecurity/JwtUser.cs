using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.JwtSecurity
{
    public static class JwtUser
    {
        public static LoginUser GetUser(ClaimsPrincipal userClaims)
        {
            try
            {
                if (userClaims.Claims.FirstOrDefault() == null) return null;
                LoginUser user = new LoginUser
                {
                    UserId = new Guid(userClaims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor).Value),
                    Role = userClaims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                    UserType = Convert.ToInt32(userClaims.Claims.FirstOrDefault(x => x.Type == "UserTypeId").Value),
                };
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Guid DecodeJwtForUserId(string jwt)
        {
            var token = new JwtSecurityToken(jwtEncodedString: jwt);
         
            return new Guid(token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor).Value);
        }
        public static LoginUser GetUserFromContext(IHttpContextAccessor context)
        {
            if (String.IsNullOrWhiteSpace(context.HttpContext.Request.Headers["Authorization"])) return null;
            LoginUser user = JsonConvert.DeserializeObject<LoginUser>(JwtTokenService.DecodeJwt(context.HttpContext.Request.Headers["Authorization"]));
            return user;
        }
    }
}
