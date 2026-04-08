using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.API.Controllers.BaseController;
using UniversitySystem.Application.DTOs.ApiResponse;
using UniversitySystem.Application.DTOs.User;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Interfaces.Common;

namespace UniversitySystem.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        public IAuthService _authService;
        public ITokenService _tokenService;
        private readonly IValidator<CreateUser> _validator;
        private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;

        public AuthController(IAuthService authService, 
            ITokenService tokenService, 
            IValidator<CreateUser> validator,
            IValidator<ResetPasswordDto> resetPasswordValidator
            )
        {
            _authService = authService;
            _tokenService = tokenService;
            _validator = validator;
            _resetPasswordValidator = resetPasswordValidator;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUser createUser)
        {
            var validation = HandleValidation(await _validator.ValidateAsync(createUser));
            if (validation != null) return validation;

            var result = await _authService.Register(createUser.Email, createUser.Password, createUser.UserRole);
            return HandleResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var result = await _authService.Login(request.Email, request.Password);
            return HandleResult(result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await _authService.ConfirmEmail(email, token);
            return HandleResult(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var result = await _authService.ForgotPassword(email);
            return HandleResult(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            var validation = HandleValidation(await _resetPasswordValidator.ValidateAsync(request));
            if (validation != null) return validation;

            var result = await _authService.ResetPassword(request);
            return HandleResult(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshToken(refreshToken);
            return HandleResult(result);
        }
    }
}
