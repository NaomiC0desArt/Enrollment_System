

using UniversitySystem.Application.Entities;

namespace UniversitySystem.Application.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<bool> IsEnrolledAsync(int studentId, int courseId);

        Task AddAsync(Enrollment enrollment);
    }
}
