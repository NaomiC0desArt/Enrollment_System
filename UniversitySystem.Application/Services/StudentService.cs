using Student_Course_System.Auxiliary;
using Student_Course_System.Entities.DTOs;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Services;
using UniversitySystem.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Data;
using UniversitySystem.Application.Entities.DTOs.Student;
using UniversitySystem.Application.Repositories.Interfaces;

namespace Student_Course_System.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

    public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<StudentResponseDto>>> GetStudents()
        {
            var students = await _unitOfWork.Students.GetAllAsync();

            if (students == null) return Result<List<StudentResponseDto>>.Failure("No students found.");

            var result = students
                .Select(s => new StudentResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    email = s.Email,
                    EnrolledCourseTitles = s.Enrollments.Select(e => e.Course.Title).ToList()
                }).ToList();

            return Result<List<StudentResponseDto>>.Success(result);

        }
        public async Task<Result<StudentResponseDto>> GetStudentById(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);

            if (student == null) return Result<StudentResponseDto>.Failure("Student not found");

            var dto = new StudentResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                email = student.Email,
                EnrolledCourseTitles = student.Enrollments
                .Select(e => e.Course.Title)
                .ToList()
            };

            return Result<StudentResponseDto>.Success(dto);
        }

        public async Task<Student> RegisterStudent(string name, string email)
        {
            Student student = new Student(name, email);
            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.CompleteAsync();

            return student;
        }

        public async Task<Result<bool>> DeleteStudent(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            if (student == null) return Result<bool>.Failure("Student not found");

            _unitOfWork.Students.Delete(student);
            await _unitOfWork.CompleteAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<Student>> UpdateStudent(int id, string name, string email)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);

            if (student == null) return Result<Student>.Failure("Student not found");

            student.Name = name;
            student.Email = email;

            _unitOfWork.Students.Update(student);
            await _unitOfWork.CompleteAsync();

            return Result<Student>.Success(student);
        }
      
        public async Task<Result<List<StudentAverage>>> TopStudents()
        {
            var students = await _unitOfWork.Students.GetStudentsWithEnrollmentsAsync();
            if (students == null || !students.Any())
                return Result<List<StudentAverage>>.Failure("No students enrolled");

            var result = students
                .Select(s => new StudentAverage
                {
                    Name = s.Name,
                    Average = Math.Round(s.Enrollments.Average(e => e.Grade), 2)
                })
                .OrderByDescending(dto => dto.Average)
                .ToList();

            return Result<List<StudentAverage>>.Success(result);
        }

    }
}
