
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.DTOs.User;
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
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            ITokenService tokenService,
            ILogger<AuthService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _tokenService = tokenService;
            _logger = logger;
            
        }

        public async Task<Result<AuthInternalResult>> Login(string email, string password)
        {
            var userInDb = await _unitOfWork.Users.GetByEmail(email);
            if (userInDb == null)
                return Result<AuthInternalResult>.Failure("Invalid email or password.");

            if (!userInDb.EmailConfirmed)
                return Result<AuthInternalResult>.Failure("Account not active. Please confirm your email.");

            if (userInDb.LockoutEnd > DateTime.UtcNow)
                return Result<AuthInternalResult>.Failure("Account is temporarily blocked.");


            bool isCorrect = BCrypt.Net.BCrypt.Verify(password, userInDb.PasswordHash);
            if (!isCorrect)
            {
                return await HandleFailedLogin<AuthInternalResult>(userInDb);
            }

            userInDb.FailedLoginAttempts = 0;
            userInDb.LockoutEnd = null;

            var accessToken = _tokenService.CreateToken(userInDb);
            var refreshToken = _tokenService.GenerateRefreshToken();

            userInDb.RefreshToken = refreshToken;
            userInDb.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("The user {Email} has logged in at {LogginTime}", userInDb.Email, DateTime.UtcNow);
            var result = new AuthInternalResult
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken,
                MustChangePassword = userInDb.MustChangePassword
            };

            return Result<AuthInternalResult>.Success(result);
        }
          
        public async Task<Result<AuthInternalResult>> RefreshToken(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshToken(refreshToken);
            if (user == null)
            {
                _logger.LogWarning("Refresh token validation failed — no matching user found at {Time}", DateTime.UtcNow);
                return Result<AuthInternalResult>.Failure("Invalid refresh token.");
            }

            if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogWarning("The token for {user} has expired.", user.Email);
                return Result<AuthInternalResult>.Failure("Refresh token expired. Please login again.");
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newAccessToken = _tokenService.CreateToken(user);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("A new refresh and access token has been created for {user} at {time}", user.Email, DateTime.UtcNow);
            return Result<AuthInternalResult>.Success(new AuthInternalResult
            {
                RefreshToken = refreshToken,
                AccessToken = newAccessToken
            });

        }
        public async Task<Result<User>> Register(string email, string password, Role rol, bool saveChanges = true)
        {
            var emailToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(emailToken)));

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(email, hashPassword, rol);

            user.EmailConfirmed = false;
            user.EmailConfirmationToken = hashedToken;
            user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);
            user.MustChangePassword = (rol.ToString().ToLower() == "student") ? true : false;

            await _unitOfWork.Users.Add(user);
            if(saveChanges) await _unitOfWork.CompleteAsync();
            _logger.LogInformation("New {Role} registered: {Email}", user.Role, user.Email);


            var emailResult = await _emailService.SendConfirmationEmailAsync(user.Email, emailToken, rol.ToString(), password);
            if (!emailResult.IsSuccess)
            {
                _logger.LogError("Confirmation email failed for {Email} after registration", user.Email);
            }
            else
            {
                _logger.LogInformation("Confirmation email sent to {Email}", user.Email);
            }
            return Result<User>.Success(user);

        }
        public async Task<Result> ForgotPassword(string email)
        {
            var user = await _unitOfWork.Users.GetByEmail(email);
            
            if (user!= null)
            {
                var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                var hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));
                user.PasswordResetToken = hashedToken;
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(2);
                await _unitOfWork.CompleteAsync();

                await _emailService.SendPasswordResetEmailAsync(user.Email, rawToken);
            }

            return Result.Success("If an account exists, an email has been sent.");
        }
        public async Task<Result> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmail(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("The user with the email: {user}, does not exists", dto.Email);
                return Result.Failure("There was an error with the validation link");
            }

            var hashedInput = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Token)));
            if (user.PasswordResetToken != hashedInput)
            {
                _logger.LogWarning("the token offered by {user} does not match", dto.Email);
                return Result.Failure("The token does not match");
            }
            if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogWarning("The token is expired");
                return Result.Failure("The token has expired");
            }

            var password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ChangePassword(password);
            user.ClearPasswordResetToken();

            user.LockoutEnd = null;
            user.FailedLoginAttempts = 0;

            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("The password for {user} has been changed at {time}", user.Email, DateTime.UtcNow);
            return Result.Success();

        }
        public async Task<Result> ConfirmEmail(string email, string token)
        {
            var user = await _unitOfWork.Users.GetByEmail(email);
            if (user == null)
            {
                _logger.LogWarning("Theres no user with an email: {email} registered in the system", email);
                return Result.Failure("The user is not registered in the system.");
            }

            var hashedInput = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

            if (user.EmailConfirmationToken != hashedInput)
            {
                _logger.LogWarning("Email confirmation token mismatch for {Email}", email);
                return Result.Failure("The token does not match.");
            }
                
            
            if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogWarning("The EmailConfirmartion token for {user} has already expired", user.Email);
                return Result.Failure("The token has already expired.");
            }
            
            user.EmailConfirmed = true;
            user.ClearConfirmationToken();
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("The account for {user} has been activated at {time}", user.Email, DateTime.UtcNow);

            return Result.Success("Email confirmed successfully!");
        }

        private async Task<Result<T>> HandleFailedLogin<T>(User user)
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                await _unitOfWork.CompleteAsync();
                _logger.LogWarning("The {email} account has been blocked at {loggTime}", user.Email, DateTime.UtcNow);
                return Result<T>.Failure("Account blocked due to multiple failed attempts.");
            }

            await _unitOfWork.CompleteAsync();
            _logger.LogWarning("User {email} has {failedAttemp} of 5", user.Email, user.FailedLoginAttempts);
            return Result<T>.Failure("Invalid credentials.");
        }

         public async Task<Result> Logout(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetByRefreshToken(refreshToken);

            if (user == null) return Result.Success();

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("{user} has logged out of their accout at {time}.", user.Email, DateTime.UtcNow);

            return Result.Success("The user has been logout.");

        }
    }
}
