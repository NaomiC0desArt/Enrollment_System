using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Common.Base;

namespace UniversitySystem.Application.Validators.Params
{
    public class PaginationParamsValidator : AbstractValidator<PaginationParams>
    {
        public PaginationParamsValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize).InclusiveBetween(1, 10)
                .WithMessage("You can only request between 1 and 10 items per page.");
        }
    }
}
