using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Interfaces;

namespace UniversitySystem.Application.Repositories.Interfaces
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
