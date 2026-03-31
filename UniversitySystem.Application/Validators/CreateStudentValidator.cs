using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Entities.DTOs.Student;
using UniversitySystem.Application.Repositories.Interfaces;

namespace UniversitySystem.Application.Validators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required");

            RuleFor(x => x.email)
                .MustAsync(async (email, cancellation) =>
                {
                    bool exists = await _unitOfWork.Students.EmailAlreadyExists(email);
                    return !exists;
                })
                .WithMessage("A student with this email already exists");
        }
    }
}
