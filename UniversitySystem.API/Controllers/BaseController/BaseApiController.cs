using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Course_System.Auxiliary;
using UniversitySystem.Application.DTOs.ApiResponse;

namespace UniversitySystem.API.Controllers.BaseController
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound(ApiResponse<string>.Fail("Resource Not Found"));

            if (result.IsSuccess)
            {
                if(result.Value is bool boolValue && boolValue)
                {
                    return NoContent();
                }

                return Ok(ApiResponse<T>.Ok(result.Value));
            }

            if (result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(ApiResponse<string>.Fail(result.Error));

            return BadRequest(ApiResponse<string>.Fail(result.Error));
        }

        protected IActionResult HandleValidation(FluentValidation.Results.ValidationResult result)
        {
            if (result.IsValid) return null;

            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

            return BadRequest(ApiResponse<object>.Fail("Validation failded", errors));
        }
    }
}
