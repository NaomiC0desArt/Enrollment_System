using FluentValidation;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Application.Repositories.Interfaces;

namespace UniversitySystem.Application.Validators
{
    public class CreateCourseValidator : AbstractValidator<CreateCourse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCourseValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(X => X.Title)
                .NotEmpty().WithMessage("The title is required.")
                .Length(3, 150).WithMessage("The title need to be between 3 to 150 characters.");

            RuleFor(x => x.Credits)
                .InclusiveBetween(1, 5).WithMessage("A course need to have 1 to 5 credits.");

            RuleFor(x => x.Title)
                .MustAsync(async (Title, cancelation) =>
                {
                    bool exists = await _unitOfWork.Courses.TitleExists(Title);
                    return !exists;
                })
                .WithMessage("Course already exists.");
        }
    }
}
