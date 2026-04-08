using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.DTOs.User;

namespace UniversitySystem.Application.Validators.User
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        private const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

        public ResetPasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("The password field is required.")
                .Matches(PasswordRegex)
                .WithMessage("Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, and one number.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
        }
    }
}
