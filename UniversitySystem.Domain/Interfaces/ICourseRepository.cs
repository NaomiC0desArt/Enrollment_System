

using UniversitySystem.Application.Entities;

namespace UniversitySystem.Application.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task AddAsync(Course course);
        void Delete(Course course);
        Task SaveChangesAsync();
        Task<bool> TitleExists(string title);

        Task<IEnumerable<Course>> GetAllWithEnrollmentsAsync();
        Task<Course?> GetCourseWithStudentsAsync(int id);
    }
}
