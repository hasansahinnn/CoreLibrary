using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Helpers
{
    public static class EnglishToTurkishConverter
    {
        public static String ConvertToEnglish( string text)
        {
            return text.ToLower().Replace('ü', 'u').Replace('ı', 'i').Replace('I', 'i').Replace('ö', 'o').Replace('ğ', 'g').Replace('ş', 's').Replace('ç', 'c').Replace('ü', 'u');
        }
    }
}
