using Student_Course_System.Auxiliary;
using Student_Course_System.Entities.DTOs;
using UniversitySystem.Application.Entities.DTOs.Course;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Result> EnrollStudent(int studentId, int courseId);
        Task<Result<(string Title, List<string> students)>> ListStudentsInCourse(int courseId);
        Task<Result<(string Name, List<CourseForListResponseDto> courses)>> ListCoursesForStudent(int studentId);

        Task<Result<List<CourseEnrollmentSummary>>> GetEnrollmentReport();

        Task<Result<List<CourseEnrollmentSummary>>> GetAverageGradePerCourse();

        Task<Result<Dictionary<int, List<CourseDto>>>> CoursesGroupedByCredits();
    }
}
