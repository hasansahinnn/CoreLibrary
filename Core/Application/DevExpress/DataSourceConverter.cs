using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.DevExpress
{
    public static class DataSourceConverter
    {
        public static DevExpressRequestModel Convert(DataSourceLoadOptions request)
        {
            DevExpressRequestModel model = new DevExpressRequestModel();
            model.Skip = request.Skip;
            model.Take = request.Take;
            if(request.Filter!=null)
            foreach(var item in request.Filter)
            {
                    if (item.ToString() == "and") continue;
                try
                {
                    var arry = JArray.Parse(item.ToString());
                    List<string> filters = JsonConvert.DeserializeObject<List<string>>(arry.ToString());
                    model.filters.Add(new FilterData(filters[0], filters[2]));
                }
                catch (Exception)
                {
                    model.filters.Add(new FilterData(request.Filter[0].ToString(), request.Filter[2].ToString()));
                }
                
            }
            if (request.Sort != null)
            {
                model.OrderColumn = request.Sort[0].Selector;
                model.IsDesc = request.Sort[0].Desc;
            }
        
            return model;
        }
    }
}
