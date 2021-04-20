using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public abstract class BaseDomainEvent
    {
        public DateTime DateOccured { get; protected set; } = DateTime.UtcNow;
    }
}
