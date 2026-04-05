using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Interfaces.Repositories;
using UniversitySystem.Persistence.Data;
using UniversitySystem.Persistence.Features.Students;

namespace UniversitySystem.Application.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UniversityDbContext _context;

        public StudentRepository(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student) => await _context.Students.AddAsync(student);

        public void Delete(Student student) => _context.Students.Remove(student);

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .AsNoTracking()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Student student) => _context.Students.Update(student);

        public async Task<PagedResult<Student>> GetStudentsAsync(StudentFilter filters)
        {

            var query = _context.Students
                .AsNoTracking()
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ApplyFilters(filters)
                .ApplySorting(filters.SortBy, filters.IsDescending);

            var totalCount = await query.CountAsync();

            var items =  await query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new PagedResult<Student>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize
            };
        }

        public async Task<List<Student>> GetStudentsWithEnrollmentsAsync()
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .Where(s => s.Enrollments.Any()) 
                .ToListAsync();
        }

        public async Task<bool> EmailAlreadyExists(string email)
        {
            return await _context.Students.AnyAsync(s => s.Email == email);
        }

        public Task<bool> EmailExistsForAnotherStudent(int id, string email)
        {
            return _context.Students.AnyAsync(s => s.Email == email && s.Id != id);
        }
    }
}
