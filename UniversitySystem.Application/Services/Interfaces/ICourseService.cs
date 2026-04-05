using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Result<PagedResult<CourseDto>>> GetCourses(CourseFilter filters);
        Task<Result<CourseDto>> GetCourseById(int id);
        Task<Result<bool>> DeleteCourse(int id);
        Task<Result<Course>> CreateCourse(string title, int credits);

    }
}
