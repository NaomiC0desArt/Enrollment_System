using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        ICourseRepository Courses { get; }

        IEnrollmentRepository Enrollments { get; }
        IUserRepository Users { get; }
        Task<int> CompleteAsync();
    }
}
