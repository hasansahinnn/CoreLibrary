using Business.ValidationRules.FluentValidation;
using Core.Caching;
using Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
namespace Core.Validation
{
    public interface ITestModelValidator
    {
        int Check(DataModel model);
    }

    public class TestDataModelValidator : ITestModelValidator
    {

        [ValidationAspect(typeof(DataModelValidator))]
        [CacheAspect(duration:10)]
        public int Check(DataModel model)
        {
            int a = 5;
            return a;
        }
    }

}
