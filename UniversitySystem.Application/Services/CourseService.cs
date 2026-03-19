using Microsoft.EntityFrameworkCore;
using Student_Course_System.Auxiliary;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Application.Repositories.Interfaces;
using UniversitySystem.Application.Services.Interfaces;

namespace UniversitySystem.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CourseDto>>> GetCourses()
        {
            var courses = await _unitOfWork.Courses.GetAllAsync();

            if (courses == null) return Result<List<CourseDto>>.Failure("No courses at the momment");

            var result = courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Credit = c.Credits,
                StudentsName = c.Enrollments.Select(e => e.Student.Name).ToList()
            }).ToList();

            return Result<List<CourseDto>>.Success(result);
        }

        public async Task<Result<CourseDto>> GetCourseById(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);

            if(course == null ) return Result<CourseDto>.Failure("Course was not found");

            var dto = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Credit = course.Credits,
                StudentsName = course.Enrollments
                .Select(e => e.Student.Name)
                .ToList()
            };

            return Result<CourseDto>.Success(dto);
        }

        public async Task<Result<bool>> DeleteCourse(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null) return Result<bool>.Failure("Course not found");

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.CompleteAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Course> CreateCourse(string title, int credits)
        {
            Course course = new Course(title, credits);
            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.CompleteAsync();

            return course;
        }


    }
}
