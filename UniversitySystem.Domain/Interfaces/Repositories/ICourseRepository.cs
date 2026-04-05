using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Domain.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<PagedResult<Course>> GetAllAsync(CourseFilter filters);
        Task<Course?> GetByIdAsync(int id);
        Task AddAsync(Course course);
        void Delete(Course course);
        Task SaveChangesAsync();
        Task<bool> TitleExists(string title);

        Task<IEnumerable<Course>> GetAllWithEnrollmentsAsync();
        Task<Course?> GetCourseWithStudentsAsync(int id);

    }
}
