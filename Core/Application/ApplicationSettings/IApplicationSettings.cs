using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application
{
    public interface IApplicationSettings
    {
        public string Version { get; }
    }
}
