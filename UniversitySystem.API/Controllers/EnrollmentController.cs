using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Services.Interfaces;

namespace UniversitySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        public IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(int studentId, int courseId)
        {
            var result = await _enrollmentService.EnrollStudent(studentId, courseId);

            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> ListStudentsInCourse(int courseId)
        {
            var result = await _enrollmentService.ListStudentsInCourse(courseId);

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> ListCoursesForStudent(int studentId)
        {
            var result = await _enrollmentService.ListCoursesForStudent(studentId);

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetEnrollmentReport()
        {
            var result = await _enrollmentService.GetEnrollmentReport();

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("average-grades")]
        public async Task<IActionResult> GetAverageGradePerCourse()
        {
            var result = await _enrollmentService.GetAverageGradePerCourse();

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("grouped-by-credits")]
        public async Task<IActionResult> CoursesGroupedByCredits()
        {
            var result = await _enrollmentService.CoursesGroupedByCredits();

            if (!result.IsSuccess) return NotFound(result.Error);
            return Ok(result.Value);
        }
    }
}
