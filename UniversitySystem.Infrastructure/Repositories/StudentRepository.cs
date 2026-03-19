using Microsoft.EntityFrameworkCore;
using Student_Course_System.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Data;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Student;
using UniversitySystem.Application.Repositories.Interfaces;

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

        public async Task<List<Student>> GetStudentsWithEnrollmentsAsync()
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .Where(s => s.Enrollments.Any()) 
                .ToListAsync();
        }


    }
}
