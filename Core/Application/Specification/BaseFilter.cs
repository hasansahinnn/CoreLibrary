using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Specification
{
    public class BaseFilter
    {
        public bool LoadChildren { get; set; } = true;
        public bool IsPagingEnabled { get; set; } = true;

        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 20;
    }
}
