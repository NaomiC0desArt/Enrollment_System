using Microsoft.EntityFrameworkCore;
using Student_Course_System.Entities.DTOs;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.DTOs.Enrollment;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Application.Exceptions;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;
using UniversitySystem.Domain.Interfaces.Repositories;

namespace UniversitySystem.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EnrollmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> EnrollStudent(int studentId, int courseId)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(studentId);
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);

            var enrollment = new Enrollment(student, course);

            await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateStudentGrade(UpdateGradeDto dto)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(dto.EnrollmentId);

            if (enrollment == null) return Result.Failure("Enrollment not found.");

            try { enrollment.UpdateGrade(dto.NewGrade); }
            catch(Exception ex) { return Result.Failure(ex.Message); }

            await _unitOfWork.CompleteAsync();

            return Result.Success();
        }
        public async Task<Result<PagedResult<EnrollmentResponseDto>>> GetEnrollments(EnrollmentFilter filters)
        {
            var pagedEntities = await _unitOfWork.Enrollments.GetEnrollmentsAsync(filters);

            var dtos = pagedEntities
                .Items.Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    StudentName = e.Student.Name,
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    EnrollmentDate = e.EnrollmentDate,
                    Grade = e.Grade
                }).ToList();

            var pagedDtos = new PagedResult<EnrollmentResponseDto>
            {
                Items = dtos,
                TotalCount = pagedEntities.TotalCount,
                PageNumber = pagedEntities.PageNumber,
                PageSize = pagedEntities.PageSize
            };

            return Result<PagedResult<EnrollmentResponseDto>>.Success(pagedDtos);
        }

        public async Task<Result<(string Title, List<string> students)>> ListStudentsInCourse(int courseId)
        {
            var data = await _unitOfWork.Courses
                .GetCourseWithStudentsAsync(courseId);

            if (data == null) throw new UserFriendlyException("Course not found.");


            var result = new
            {
                Title = data.Title,
                studentNames = data.Enrollments.Select(e => e.Student.Name).ToList()
            };

            return Result<(string, List<string>)>.Success((result.Title, result.studentNames));

        }

        public async Task<Result<(string Name, List<CourseForListResponseDto> courses)>> ListCoursesForStudent(int studentId)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(studentId);

            if (student == null) throw new UserFriendlyException("Student not found");

            var result = new
            {
                student.Name,
                ListOfCourses = student.Enrollments.Select(e => new CourseForListResponseDto
                {
                    Title = e.Course.Title,
                    Credits = e.Course.Credits
                }).ToList()
            };

            return Result<(string, List<CourseForListResponseDto>)>.Success((result.Name, result.ListOfCourses));
        }

        public async Task<Result<List<CourseEnrollmentSummary>>> GetEnrollmentReport()
        {

            var courses = await _unitOfWork.Courses
                .GetAllWithEnrollmentsAsync();

            if (courses == null)
                throw new UserFriendlyException("No courses available.");

            var report = courses.Select(c => new CourseEnrollmentSummary
            {
                CourseTitle = c.Title,
                StudentCount = c.Enrollments.Count()
            }).ToList();


            return Result<List<CourseEnrollmentSummary>>.Success(report);
        }

        public async Task<Result<List<CourseEnrollmentSummary>>> GetAverageGradePerCourse()
        {
            var courses = await _unitOfWork.Courses
                .GetAllWithEnrollmentsAsync();

            if (courses == null)
                throw new UserFriendlyException("No courses available.");


            var report = courses.Select(c => new CourseEnrollmentSummary
            {
                CourseTitle = c.Title,
                StudentCount = c.Enrollments.Count(),
                StudentGrade = Math.Round(c.Enrollments.Average(e => e.Grade), 2)
            }).ToList();


            return Result<List<CourseEnrollmentSummary>>.Success(report);
        }

        public async Task<Result<Dictionary<int, List<CourseDto>>>> CoursesGroupedByCredits()
        {
            var filters = new CourseFilter();

            var courses = await _unitOfWork.Courses.GetAllAsync(filters);

            if (courses == null || !courses.Items.Any())
                throw new UserFriendlyException("No courses available.");


            var result = courses.Items.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Credit = c.Credits
            }).GroupBy(c => c.Credit)
                .ToDictionary(g => g.Key, g => g.ToList());


            return Result<Dictionary<int, List<CourseDto>>>.Success(result);
        }

    }
}
