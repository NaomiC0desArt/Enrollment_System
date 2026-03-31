using Student_Course_System.Auxiliary;
using UniversitySystem.Application.DTOs.Common;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Student;
using UniversitySystem.Domain.Common;

namespace UniversitySystem.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Result<List<StudentResponseDto>>> GetStudents();
        Task<Result<StudentResponseDto>> GetStudentById(int id);
        Task<Result<Student>> RegisterStudent(string name, string email);
        Task<Result<Student>> UpdateStudent(int id, string name, string email);
        Task<Result<bool>> DeleteStudent(int id);
        Task<Result<PagedResult<StudentResponseDto>>> GetStudentsPaged(StudentFilter filters);
    }
}
