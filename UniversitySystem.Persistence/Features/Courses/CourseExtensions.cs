using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Persistence.Features.Courses
{
    public static class CourseExtensions
    {
        public static IQueryable<Course> ApplyFilters(this IQueryable<Course> query, CourseFilter filters)
        {
            if (!string.IsNullOrWhiteSpace(filters.Title))
                query = query.Where(c => c.Title.Contains(filters.Title));

            if (filters.Credits != null)
                query = query.Where(c => c.Credits.Equals(filters.Credits));

            return query;
        }

        public static IQueryable<Course> ApplySorting(this IQueryable<Course> query, string? sortBy, bool isDecending)
        {
            return sortBy?.ToLower() switch
            {
                "title" => isDecending ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
                "credits" => isDecending ? query.OrderByDescending(c => c.Credits) : query.OrderBy(c => c.Credits),
                _ => query.OrderBy(c => c.Id)
            };

        }
    }
}
