using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.Application.Validators.Params
{
    public class EnrollmentFilterValidator : AbstractValidator<EnrollmentFilter>
    {
        public EnrollmentFilterValidator()
        {
            Include(new PaginationParamsValidator());

            RuleFor(x => x.MinGrade)
                .LessThanOrEqualTo(x => x.MaxGrade)
                .When(x => x.MinGrade.HasValue && x.MaxGrade.HasValue)
                .WithMessage("Min grade cannot be higher than Max grade.");

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("The start date must be before the end date.");
        }
    }
}
