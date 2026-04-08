using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.DTOs.User;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<User>> Register(string email, string password, Role rol);
        Task<Result<AuthResponseDto>> Login(string email, string password);
        Task<Result> ConfirmEmail(string email, string token);
        Task<Result> ForgotPassword(string email);
        Task<Result> ResetPassword(ResetPasswordDto dto);
        Task<Result<AuthResponseDto>> RefreshToken(string refreshToken);
    }
}
