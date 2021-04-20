using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.JwtSecurity
{
    public static class JwtTokenService
    {
        public static string GenerateJSONWebToken(Guid id, string role, int userTypeId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfigProvider._config.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Actor, id.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim("UserTypeId", userTypeId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };
            var token = new JwtSecurityToken( 
                issuer: JwtConfigProvider._config.Issuer,
                audience: JwtConfigProvider._config.Audience,
                claims,
                expires: DateTime.Now.AddDays(JwtConfigProvider._config.AccessTokenExpiration),
                signingCredentials: credentials
                );
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        public static string GenerateJSONRefreshToken(Guid id, string role,  int userTypeId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfigProvider._config.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Actor, id.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim("UserTypeId", userTypeId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };
            var token = new JwtSecurityToken(
               issuer: JwtConfigProvider._config.Issuer,
                audience: JwtConfigProvider._config.Audience,
                claims,
                expires: DateTime.Now.AddDays(JwtConfigProvider._config.AccessTokenExpiration),
                signingCredentials: credentials
                );
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }


        public static string DecodeJwt(string jwt)
        {
            var token = new JwtSecurityToken(jwtEncodedString: jwt.Split(' ')[1]);
            var loginUser = new
            {
                UserId = new Guid(token.Claims.Where(x => x.Type == ClaimTypes.Actor).FirstOrDefault().Value),
                Role = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                UserType = Convert.ToInt32(token.Claims.Where(x => x.Type == "UserTypeId").FirstOrDefault().Value),
                ExpiresDate = token.Claims.Where(x => x.Type == "exp").FirstOrDefault().Value
            };
            return JsonConvert.SerializeObject(loginUser);
        }
        public static bool IsValidExpire(string tokens)
        {
            var token = new JwtSecurityToken(jwtEncodedString: tokens);
            string loginUser = token.Claims.Where(x => x.Type == "exp").FirstOrDefault().Value;
            DateTime tokenExpiryTime = UnixTimeStampToDateTime(Convert.ToDouble(loginUser));
            return tokenExpiryTime > DateTime.Now;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}

