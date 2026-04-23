using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.API.Controllers.BaseController;
using UniversitySystem.Application.DTOs.ApiResponse;
using UniversitySystem.Application.DTOs.Student;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Student;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Domain.Common.Filters;

namespace UniversitySystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseApiController
    {
        public readonly IStudentService _studentService;
        private readonly IValidator<CreateStudent> _validator;
        private readonly IValidator<UpdateStudent> _updateValidator;
        private readonly IValidator<StudentFilter> _paginationValidator;

        public StudentController(IStudentService studentService, IValidator<CreateStudent> validator, IValidator<UpdateStudent> updateValidator, IValidator<StudentFilter> paginationValidator)
        {
            _studentService = studentService;
            _validator = validator;
            _updateValidator = updateValidator;
            _paginationValidator = paginationValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await _studentService.GetStudents());

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return HandleResult(await _studentService.GetStudentById(id));

        }

        [HttpGet("GetStudents-Paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] StudentFilter filters)
        {
            var validationResult = await _paginationValidator.ValidateAsync(filters);

            if (!validationResult.IsValid) return HandleValidation(validationResult);

            var result = await _studentService.GetStudentsPaged(filters);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudent student)
        {
            var validation = HandleValidation(await _validator.ValidateAsync(student));
            if (validation != null) return validation;

            var result = await _studentService.RegisterStudent(student.Name, student.email);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Value.Id },
                ApiResponse<Student>.Ok(result.Value, "Student created successfully!"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudent student)
        {
            if (id != student.Id)
            {
                return BadRequest(ApiResponse<object>.Fail("ID mismatch between route and body."));
            }

            var validation = HandleValidation(await _updateValidator.ValidateAsync(student));
            if (validation != null) return validation;

            return HandleResult(await _studentService.UpdateStudent(id, student.Name, student.Email));

        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult(await _studentService.DeleteStudent(id));

        }
    }
}
