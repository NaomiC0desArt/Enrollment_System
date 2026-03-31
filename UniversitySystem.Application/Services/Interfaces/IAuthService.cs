using Student_Course_System.Auxiliary;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<User>> Register(string email, string password, Role rol);
        Task<Result<User>> Login(string email, string password);
    }
}
