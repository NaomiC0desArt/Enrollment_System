

using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
