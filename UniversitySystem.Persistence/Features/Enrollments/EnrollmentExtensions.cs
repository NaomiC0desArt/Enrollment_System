using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.Persistence.Features.Enrollments
{
    public static class EnrollmentExtensions
    {
        public static IQueryable<Enrollment> ApplyFilters(this IQueryable<Enrollment> query, EnrollmentFilter filters)
        {
            if (filters.StudentId.HasValue)
                query = query.Where(e => e.StudentId == filters.StudentId);

            if (filters.CourseId.HasValue)
                query = query.Where(e => e.CourseId == filters.CourseId);

            if (filters.MinGrade.HasValue)
                query = query.Where(e => e.Grade >= filters.MinGrade);

            if (filters.MaxGrade.HasValue)
                query = query.Where(e => e.Grade <= filters.MaxGrade);

            if (filters.FromDate.HasValue)
                query = query.Where(e => e.EnrollmentDate >= filters.FromDate);

            if (filters.ToDate.HasValue)
                query = query.Where(e => e.EnrollmentDate <= filters.ToDate);

            return query;
        }

        public static IQueryable<Enrollment> ApplySorting(this IQueryable<Enrollment> query, string? sortBy, bool isDecending)
        {
            return sortBy?.ToLower() switch
            {
                "grade" => isDecending ? query.OrderByDescending(e => e.Grade) : query.OrderBy(e => e.Grade),
                "date" => isDecending ? query.OrderByDescending(e => e.EnrollmentDate) : query.OrderBy(e => e.EnrollmentDate),
                "studentName" => isDecending ? query.OrderByDescending(e => e.Student.Name) : query.OrderBy(e => e.Student.Name),
                _ => query.OrderBy(e => e.Id)
            };
        }

    }
}
