using Microsoft.EntityFrameworkCore;
using Student_Course_System.Auxiliary;
using Student_Course_System.Entities.DTOs;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Application.Exceptions;
using UniversitySystem.Application.Repositories.Interfaces;
using UniversitySystem.Application.Services.Interfaces;

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
            var courses = await _unitOfWork.Courses.GetAllAsync();

            if (courses == null || !courses.Any())
                throw new UserFriendlyException("No courses available.");


            var result = courses.Select(c => new CourseDto
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
