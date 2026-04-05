

using FluentValidation;
using UniversitySystem.Application.DTOs.Student;
using UniversitySystem.Domain.Interfaces.Repositories;

namespace UniversitySystem.Application.Validators
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id cannot be less than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required");

            RuleFor(x => x.Email)
                .MustAsync(async (model, email, cancellation) =>
                {
                    bool exists = await _unitOfWork.Students.EmailExistsForAnotherStudent(model.Id, email);
                    return !exists;
                })
                .WithMessage("This emaul is already taken by another student");
        }
    }
}
