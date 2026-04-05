using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.Application.Validators.Params
{
    public class StudentFilterValidator : AbstractValidator<StudentFilter>
    {
        public StudentFilterValidator()
        {
            Include(new PaginationParamsValidator());

            RuleFor(x => x.Name).MinimumLength(3)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .MaximumLength(50);

            RuleFor(x => x.EmailDomain)
                .Must(e => e == null || e.Contains('.'))
                .WithMessage("Invalid domain format.");
        }
    }
}
