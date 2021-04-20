using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Core.Application;
using Core.Intercepters;
using FluentValidation;

namespace Core.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil"/*AspectMessages.WrongValidationType*/);
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            List<string> errorMessages = new List<string>();

            foreach (var entity in entities)
            {
                errorMessages.AddRange(ValidationTool.Validate(validator, entity));
            }
            if (errorMessages.Count > 0)
            {
                Exceptions.Messages(errorMessages);
            }
        }
    }
}
