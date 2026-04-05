using Student_Course_System.Entities.DTOs;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.DTOs.Enrollment;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Domain.Common.Base;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Result> EnrollStudent(int studentId, int courseId);
        Task<Result<(string Title, List<string> students)>> ListStudentsInCourse(int courseId);
        Task<Result<(string Name, List<CourseForListResponseDto> courses)>> ListCoursesForStudent(int studentId);
        Task<Result> UpdateStudentGrade(UpdateGradeDto dto);
        Task<Result<List<CourseEnrollmentSummary>>> GetEnrollmentReport();

        Task<Result<List<CourseEnrollmentSummary>>> GetAverageGradePerCourse();

        Task<Result<Dictionary<int, List<CourseDto>>>> CoursesGroupedByCredits();

        Task<Result<PagedResult<EnrollmentResponseDto>>> GetEnrollments(EnrollmentFilter filters);
    }
}
