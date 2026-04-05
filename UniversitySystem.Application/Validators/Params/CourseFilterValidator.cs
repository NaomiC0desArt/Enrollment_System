using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.Application.Validators.Params
{
    public class CourseFilterValidator : AbstractValidator<CourseFilter>
    {
        public CourseFilterValidator()
        {
            Include(new PaginationParamsValidator());

            RuleFor(x => x.Title).MinimumLength(4)
                .When(x => !string.IsNullOrEmpty(x.Title))
                .MaximumLength(150);

            RuleFor(x => x.Credits)
                .InclusiveBetween(1, 6)
                .WithMessage("There are no courses with more than 6 credits.");
        }
    }
}
