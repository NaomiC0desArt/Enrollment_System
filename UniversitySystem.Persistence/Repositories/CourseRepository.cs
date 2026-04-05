using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Interfaces.Repositories;
using UniversitySystem.Persistence.Data;
using UniversitySystem.Persistence.Features.Courses;

namespace UniversitySystem.Persistence.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversityDbContext _context;

        public CourseRepository(UniversityDbContext context) => _context = context;
        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
        }

        public void Delete(Course course) => _context.Courses.Remove(course);

        public async Task<PagedResult<Course>> GetAllAsync(CourseFilter filters)
        {
            var query = _context.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .ApplyFilters(filters)
                .ApplySorting(filters.SortBy, filters.IsDescending);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PagedResult<Course>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize
            };

        }

        public async Task<IEnumerable<Course>> GetAllWithEnrollmentsAsync()
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetCourseWithStudentsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<bool> TitleExists(string title)
        {
            return _context.Courses.AnyAsync(c => c.Title == title);
        }
    }
}
