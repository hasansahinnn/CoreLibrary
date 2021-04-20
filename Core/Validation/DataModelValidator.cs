using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class DataModelValidator : AbstractValidator<DataModel>
    {
        public DataModelValidator()
        {
            RuleFor(p => p.ModelId).NotEmpty()
            .WithMessage("Empty Id");
            RuleFor(p => p.Name)
                .Cascade(CascadeMode.Continue)
                .NotEmpty().WithMessage("{PropertyName} should be not empty.")
                .Length(2, 25);
        }
    }
}
