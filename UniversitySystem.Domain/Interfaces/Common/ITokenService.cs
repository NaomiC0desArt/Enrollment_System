using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Domain.Interfaces.Common
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
