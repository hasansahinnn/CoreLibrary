using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DevExpress
{
    public class DevExpressRequestModel
    {
        public List<FilterData> filters = new List<FilterData>();

        public string OrderColumn  = "";
        
        public bool IsDesc   = false;

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool HasParent { get; set; }
       
    }
    public class FilterData
    {
        public string PropName { get; set; }
        public string Value { get; set; }

        public FilterData(string propName, string value)
        {
            PropName = propName;
            Value = value;
        }
    }
}
