using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.Domain.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        Task SaveChangesAsync();
        Task<List<Student>> GetStudentsWithEnrollmentsAsync();
        Task<bool> EmailAlreadyExists(string email);
        Task<bool> EmailExistsForAnotherStudent(int id, string email);
        Task<PagedResult<Student>> GetStudentsAsync(StudentFilter filters);
    }
}
