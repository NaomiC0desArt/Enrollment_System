
using UniversitySystem.Application.Repositories;
using UniversitySystem.Domain.Interfaces.Repositories;
using UniversitySystem.Infrastructure.Repositories;
using UniversitySystem.Persistence.Data;
using UniversitySystem.Persistence.Repositories;

namespace UniversitySystem.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniversityDbContext _context;
        public IStudentRepository Students { get; private set; }
        public ICourseRepository Courses { get; private set; }
        public IEnrollmentRepository Enrollments { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(UniversityDbContext context)
        {
            _context = context;
            Students = new StudentRepository(_context);
            Courses = new CourseRepository(_context);
            Enrollments = new EnrollmentRepository(_context);
            Users = new UserRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
