using Student_Course_System.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Course;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Result<List<CourseDto>>> GetCourses();
        Task<Result<CourseDto>> GetCourseById(int id);
        Task<Result<bool>> DeleteCourse(int id);
        Task<Course> CreateCourse(string title, int credits);

    }
}
