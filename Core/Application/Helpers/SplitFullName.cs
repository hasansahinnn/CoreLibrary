using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Application.Helpers
{
    public static class SplitFullName
    {
        public static (string, string) Split(string fullName)
        {
            int idx = fullName.IndexOf(' ');
            if (fullName.Count(Char.IsWhiteSpace) > 1)
            {
                idx = fullName.LastIndexOf(' ');
            }

            if (idx != -1)
            {
                return (fullName.Substring(0, idx), fullName.Substring(idx + 1));
            }
            return (fullName, "");
        }
    }
}
