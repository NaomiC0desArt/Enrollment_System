using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Data;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Repositories.Interfaces;

namespace UniversitySystem.Application.Repositories
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

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                .AsNoTracking()
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .ToListAsync();

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

    }
}
