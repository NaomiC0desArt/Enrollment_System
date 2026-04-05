using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.Domain.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<bool> IsEnrolledAsync(int studentId, int courseId);
        Task<Enrollment?> GetByIdAsync(int id);

        Task AddAsync(Enrollment enrollment);
        Task<PagedResult<Enrollment>> GetEnrollmentsAsync(EnrollmentFilter filters);
    }
}
