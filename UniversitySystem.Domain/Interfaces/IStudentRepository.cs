
using UniversitySystem.Application.DTOs.Common;
using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Common;

namespace UniversitySystem.Application.Repositories.Interfaces
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
