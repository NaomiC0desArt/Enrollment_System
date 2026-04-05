using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniversitySystem.Application.DTOs.User;
using UniversitySystem.Domain.Interfaces.Repositories;

namespace UniversitySystem.Application.Validators.User
{
    public class RegisterUserValidator : AbstractValidator<CreateUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

        public RegisterUserValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email is required")
                .EmailAddress().WithMessage("The email its in the incorrect format.");

            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellation) =>
                {
                    bool exists = await _unitOfWork.Users.ExistByEmail(email);
                    return !exists;
                })
                .WithMessage("The email is already in use.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Matches(PasswordRegex)
                .WithMessage("Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, and one number.");

        }
    }
}
