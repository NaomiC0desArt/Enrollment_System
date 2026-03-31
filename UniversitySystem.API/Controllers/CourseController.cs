using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.API.Controllers.BaseController;
using UniversitySystem.Application.DTOs.ApiResponse;
using UniversitySystem.Application.Entities;
using UniversitySystem.Application.Entities.DTOs.Course;
using UniversitySystem.Application.Services.Interfaces;

namespace UniversitySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseApiController
    {
        public ICourseService _courseService;
        private readonly IValidator<CreateCourse> _validator;


        public CourseController(ICourseService courseService, IValidator<CreateCourse> validator)
        {
            _courseService = courseService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await _courseService.GetCourses());

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           return HandleResult( await _courseService.GetCourseById(id));

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourse course)
        {
            var validationResult = await _validator.ValidateAsync(course);

            var validationResponse = HandleValidation(validationResult);
            if (validationResponse != null) return validationResponse;

            return HandleResult(await _courseService.CreateCourse(course.Title, course.Credits));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return HandleResult( await _courseService.DeleteCourse(id));

        }
    }
}
