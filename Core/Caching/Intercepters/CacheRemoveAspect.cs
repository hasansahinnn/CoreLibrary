using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Core.AutoFac;
using Core.Intercepters;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Caching
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheManager _cacheManager;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            _cacheManager = AutoFacManager.Resolve<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
