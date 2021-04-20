using System;
using System.Collections.Generic;
using System.Text;

namespace Core.JwtSecurity
{
    public class LoginUser
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int UserType { get; set; }
        public LoginUser()
        {

        }
        public LoginUser(Guid userId, string role, string token, string refreshToken, int userType)
        {
            UserId = userId;
            Role = role;
            Token = token;
            RefreshToken = refreshToken;
            UserType = userType;
        }
    }
}
