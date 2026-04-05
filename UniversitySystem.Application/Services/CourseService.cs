using Microsoft.EntityFrameworkCore;
using System.Net;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Application.Exceptions;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Interfaces.Repositories;

namespace UniversitySystem.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PagedResult<CourseDto>>> GetCourses(CourseFilter filters)
        {
            var pagedEntities =  await _unitOfWork.Courses.GetAllAsync(filters);

            var dtos = pagedEntities.Items.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Credit = c.Credits,
                StudentsName = c.Enrollments.Select(e => e.Student.Name).ToList()
            }).ToList();

            var pagedDtos = new PagedResult<CourseDto>
            {
                Items = dtos,
                TotalCount = pagedEntities.TotalCount,
                PageNumber = pagedEntities.PageNumber,
                PageSize = pagedEntities.PageSize
            };

            return Result<PagedResult<CourseDto>>.Success(pagedDtos);
        }

        public async Task<Result<CourseDto>> GetCourseById(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);

            if(course == null ) throw new UserFriendlyException("Course not found", HttpStatusCode.NotFound);

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
            if (course == null) throw new UserFriendlyException("Course not found", HttpStatusCode.NotFound);

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.CompleteAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<Course>> CreateCourse(string title, int credits)
        {
            Course course = new Course(title, credits);
            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.CompleteAsync();

            return Result<Course>.Success(course);
        }


    }
}
