using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Interfaces.Repositories;
using UniversitySystem.Persistence.Data;
using UniversitySystem.Persistence.Features.Enrollments;

namespace UniversitySystem.Persistence.Repositories
{
    internal class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly UniversityDbContext _context;

        public EnrollmentRepository(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
        }

        public async Task<bool> IsEnrolledAsync(int studentId, int courseId)
        {
            var result = await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            return result;

        }

        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _context.Enrollments.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<PagedResult<Enrollment>> GetEnrollmentsAsync(EnrollmentFilter filters)
        {
            var query = _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ApplyFilters(filters)
                .ApplySorting(filters.SortBy, filters.IsDescending);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PagedResult<Enrollment>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize
            };

        }
    }
}
