using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.SeriLog
{
    public class ErrorLog 
    {
        public int? UserId { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
