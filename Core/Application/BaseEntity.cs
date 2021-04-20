using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application
{
    public abstract class BaseEntity
    {

        [Key]
        public Guid Id { get; set; }
        private readonly List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
