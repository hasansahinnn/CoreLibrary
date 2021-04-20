using Castle.DynamicProxy;
using Core.Intercepters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Core.Transaction
{
    public class TransactionScopeInterceptor : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception e)
                {
                    transactionScope.Dispose();
                    throw;
                }
            }
        }
    }
}
