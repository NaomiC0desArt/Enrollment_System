using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.DTOs.User;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<User>> Register(string email, string password, Role rol, bool saveChanges = true);
        Task<Result<AuthInternalResult>> Login(string email, string password);
        Task<Result> ConfirmEmail(string email, string token);
        Task<Result> ForgotPassword(string email);
        Task<Result> ResetPassword(ResetPasswordDto dto);
        Task<Result<AuthInternalResult>> RefreshToken(string refreshToken);
        Task<Result> Logout(string refreshToken);
        
    }
}
