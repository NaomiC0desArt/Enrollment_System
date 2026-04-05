using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task Add(User user);
        Task<bool> ExistByEmail(string email);
    }
}
