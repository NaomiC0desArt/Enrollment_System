using FluentValidation;
using UniversitySystem.Application.DTOs.Enrollment;
using UniversitySystem.Domain.Interfaces.Repositories;

namespace UniversitySystem.Application.Validators.Enrollment
{
    public class EnrollStudentValidator : AbstractValidator<EnrollStudentRequest>
    {

        private readonly IUnitOfWork _unitOfWork;

        public EnrollStudentValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Id cannot be less than 0.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Id cannot be less than 0.");

            RuleFor(x => x.StudentId)
                .MustAsync(async (id, cancellation) =>await _unitOfWork.Students.GetByIdAsync(id) != null)
                .WithMessage("The student does not exists.");

            RuleFor(x => x.CourseId)
                .MustAsync(async (id, cancellation) => await _unitOfWork.Courses.GetByIdAsync(id) != null)
                .WithMessage("The course does not exists");

            RuleFor(x => x)
                .MustAsync(async (request, cancellation) => !await _unitOfWork.Enrollments.IsEnrolledAsync(request.StudentId, request.CourseId))
                .WithMessage("Student is already enrolled in this course.");

        }
    }
}
