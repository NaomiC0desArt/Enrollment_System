using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.API.Controllers.BaseController;
using UniversitySystem.Application.DTOs.ApiResponse;
using UniversitySystem.Application.DTOs.Enrollment;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Services.Interfaces;

namespace UniversitySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseApiController
    {
        public IEnrollmentService _enrollmentService;
        private readonly IValidator<EnrollStudentRequest> _validator;

        public EnrollmentController(IEnrollmentService enrollmentService, IValidator<EnrollStudentRequest> validator)
        {
            _enrollmentService = enrollmentService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody]EnrollStudentRequest request)
        {
            var validation = HandleValidation(await _validator.ValidateAsync(request));
            if (validation != null) return validation;

            var result = await _enrollmentService.EnrollStudent(request.StudentId, request.CourseId);
            return Ok(ApiResponse<object>.Ok(null, "Student enrolled successfully!!"));
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> ListStudentsInCourse(int courseId)
        {
            return HandleResult(await _enrollmentService.ListStudentsInCourse(courseId));
           
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> ListCoursesForStudent(int studentId)
        {
            return HandleResult(await _enrollmentService.ListCoursesForStudent(studentId));
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetEnrollmentReport()
        {
            return HandleResult( await _enrollmentService.GetEnrollmentReport());
        }

        [HttpGet("average-grades")]
        public async Task<IActionResult> GetAverageGradePerCourse()
        {
           return HandleResult(await _enrollmentService.GetAverageGradePerCourse());

        }

        [HttpGet("grouped-by-credits")]
        public async Task<IActionResult> CoursesGroupedByCredits()
        {
            return HandleResult( await _enrollmentService.CoursesGroupedByCredits());

        }
    }
}
