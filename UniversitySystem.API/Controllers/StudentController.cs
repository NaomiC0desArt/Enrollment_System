using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Course_System.Services;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Student;
using UniversitySystem.Application.Services;
using UniversitySystem.Application.Services.Interfaces;

namespace UniversitySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentService.GetStudents();

            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Value);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentById(id);

            if (!student.IsSuccess) return NotFound(student.Error);
            return Ok(student.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudent student)
        {
           Student createdStudent =  await _studentService.RegisterStudent(student.Name, student.email);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateStudent student)
        {
            var result = await _studentService.UpdateStudent(id, student.Name, student.email);

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteStudent = await _studentService.DeleteStudent(id);

            if (!deleteStudent.IsSuccess) return NotFound(deleteStudent.Error);
            return NoContent();
        }
    }
}
