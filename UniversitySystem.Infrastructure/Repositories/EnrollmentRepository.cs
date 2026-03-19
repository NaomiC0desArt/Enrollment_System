using Microsoft.EntityFrameworkCore;
using Student_Course_System.Auxiliary;
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
            var result =  await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            return result;

        }
    }
}
