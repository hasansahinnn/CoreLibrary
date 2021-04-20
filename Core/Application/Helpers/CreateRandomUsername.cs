using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Helpers
{
    public static class RandomUsername
    {
        public static string Create(string firstName)
        {
            Random r = new Random();
            return firstName + r.Next(1, 1000);
        }
    }
}
