using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.DTOs.User;
using UniversitySystem.Application.Exceptions;
using UniversitySystem.Application.Interfaces.Auth;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Domain.Interfaces.Common;
using UniversitySystem.Domain.Interfaces.Repositories;

namespace UniversitySystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<Result<AuthResponseDto>> Login(string email, string password)
        {
            var userInDb = await _unitOfWork.Users.GetByEmail(email);
            if (userInDb == null)
                return Result<AuthResponseDto>.Failure("Invalid email or password.");

            if (!userInDb.EmailConfirmed)
                return Result<AuthResponseDto>.Failure("Account not active. Please confirm your email.");

            if (userInDb.LockoutEnd > DateTime.UtcNow)
                return Result<AuthResponseDto>.Failure("Account is temporarily blocked.");

            bool isCorrect = BCrypt.Net.BCrypt.Verify(password, userInDb.PasswordHash);
            if (!isCorrect)
            {
                return await HandleFailedLogin<AuthResponseDto>(userInDb);
            }

            userInDb.FailedLoginAttempts = 0;
            userInDb.LockoutEnd = null;

            var accessToken = _tokenService.CreateToken(userInDb);
            var refreshToken = _tokenService.GenerateRefreshToken();

            userInDb.RefreshToken = refreshToken;
            userInDb.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.CompleteAsync();

            var result = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Result<AuthResponseDto>.Success(result);
        }
          
        public async Task<Result<AuthResponseDto>> RefreshToken(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshToken(refreshToken);
            if (user == null) return Result<AuthResponseDto>.Failure("Invalid refresh token.");

            if (user.RefreshTokenExpiry < DateTime.UtcNow)
                return Result<AuthResponseDto>.Failure("Refresh token expired. Please login again.");

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newAccessToken = _tokenService.CreateToken(user);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.CompleteAsync();

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });

        }
        public async Task<Result<User>> Register(string email, string password, Role rol)
        {
            var emailToken = Guid.NewGuid().ToString();

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(email, hashPassword, rol);

            user.EmailConfirmed = false;
            user.EmailConfirmationToken = emailToken;
            user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);

            await _unitOfWork.Users.Add(user);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendConfirmationEmailAsync(user.Email, user.EmailConfirmationToken);

            return Result<User>.Success(user);

        }
        public async Task<Result> ForgotPassword(string email)
        {
            var user = await _unitOfWork.Users.GetByEmail(email);

            
            if (user!= null)
            {
                var token = Guid.NewGuid().ToString();
                user.PasswordResetToken = token;
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(2);
                await _unitOfWork.CompleteAsync();

                await _emailService.SendPasswordResetEmailAsync(user.Email, token);
            }

            return Result.Success("If an account exists, an email has been sent.");
        }
        public async Task<Result> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmail(dto.Email);
            if (user == null) return Result.Failure("There was an error with the validation link");
            if (user.PasswordResetToken != dto.Token) return Result.Failure("The token does not match");
            if (user.PasswordResetTokenExpiry < DateTime.UtcNow) return Result.Failure("The token has expired");

            var password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ChangePassword(password);
            user.ClearPasswordResetToken();

            user.LockoutEnd = null;
            user.FailedLoginAttempts = 0;

            await _unitOfWork.CompleteAsync();
            return Result.Success();

        }
        public async Task<Result> ConfirmEmail(string email, string token)
        {
            var user = await _unitOfWork.Users.GetByEmail(email);
            if (user == null)
                return Result.Failure("The user is not registered in the system.");

            
            if (user.EmailConfirmationToken != token)
                return Result.Failure("The token does not match.");

            
            if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
                return Result.Failure("The token has already expired.");

            
            user.EmailConfirmed = true;
            user.ClearConfirmationToken();
            await _unitOfWork.CompleteAsync();

            return Result.Success("Email confirmed successfully!");
        }

        private async Task<Result<T>> HandleFailedLogin<T>(User user)
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                await _unitOfWork.CompleteAsync();
                return Result<T>.Failure("Account blocked due to multiple failed attempts.");
            }

            await _unitOfWork.CompleteAsync();
            return Result<T>.Failure("Invalid credentials.");
        }
    }
}
