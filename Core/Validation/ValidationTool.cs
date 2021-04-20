using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Validation
{
    public static class ValidationTool
    {
        public static List<string> Validate(IValidator validator,object entity)
        {
            var context = new ValidationContext<object>(entity);
            var validationResult = validator.Validate(context);
            List<string> validationMessages = new List<string>();
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure failure in validationResult.Errors)
                    validationMessages.Add(failure.ErrorMessage);
                return validationMessages;
            }
            return new List<string>();
        }
        public static (bool,List<string>) Validate<T,Y>(Y model) where T : AbstractValidator<Y>, new() 
        {
            T validator = new T();
            List<string> validationMessages = new List<string>();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure failure in validationResult.Errors)
                    validationMessages.Add(failure.ErrorMessage);
            }
            return (validationMessages.Count == 0, validationMessages);
        }
    }
}
