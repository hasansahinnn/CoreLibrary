using Ardalis.Specification;
using Core.Application.DevExpress;
using Core.Application.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DevExpress
{
    public class CategoryController 
    {
        public readonly IAsyncRepository<DataModel> repository;
        public CategoryController(IAsyncRepository<DataModel> _repository)
        {
            repository = _repository;
        }

        [HttpGet("GetDevExpressTable")]
        public async Task<List<DataModel>> GetDevExpressTable(DataSourceLoadOptions loadOptions)
        {
            List<DataModel> categories = await repository.ListSpecificationAsync(new PanelCategoryGetSpecification(DataSourceConverter.Convert(loadOptions)));
            return categories;
        }
    }
    public class PanelCategoryGetSpecification : Specification<DataModel>
    {
        public PanelCategoryGetSpecification(DevExpressRequestModel filter)
        {
            foreach (var item in filter.filters)
            {
                switch (item.PropName)
                {
                    case "categoryName":
                        if (!string.IsNullOrEmpty(item.Value))
                            Query.Where(x => x.Name.Contains(item.Value));
                        break;
                    default:
                        break;
                }
            }
            switch (filter.OrderColumn)
            {
                case "categoryName":
                    if (filter.IsDesc)
                        Query.OrderByDescending(x => x.Name);
                    else
                        Query.OrderBy(x => x.Name);
                    break;

            }

            // Query.Include(x => x.ParentCategory);

            Query.Skip(filter.Skip)
                 .Take(filter.Take);

        }
    }

}
