using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.DTOs.Common;
using UniversitySystem.Application.Entities;

namespace UniversitySystem.Application.Features.Students
{
    public static class StudentExtensions
    {
        public static IQueryable<Student> ApplyFilters(this IQueryable<Student> query, StudentFilter filters)
        {
            if (!string.IsNullOrWhiteSpace(filters.Name))
                query = query.Where(s => s.Name.Contains(filters.Name));

            if (!string.IsNullOrWhiteSpace(filters.EmailDomain))
                query = query.Where(s => s.Email.Contains(filters.EmailDomain));

            return query;
        }

        public static IQueryable<Student> ApplySorting(this IQueryable<Student> query, string? sortBy, bool isDecending)
        {
            return sortBy?.ToLower() switch
            {
                "name" => isDecending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "email" => isDecending ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email),
                _ => query.OrderBy(s => s.Id)
            };

        }
    }
}
