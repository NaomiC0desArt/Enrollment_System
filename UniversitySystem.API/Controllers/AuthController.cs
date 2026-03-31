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

namespace UniversitySystem.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        public IAuthService _authService;
        public ITokenService _tokenService;
        private readonly IValidator<CreateUser> _validator;

        public AuthController(IAuthService authService, ITokenService tokenService, IValidator<CreateUser> validator)
        {
            _authService = authService;
            _tokenService = tokenService;
            _validator = validator;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUser createUser)
        {
            var validation = HandleValidation(await _validator.ValidateAsync(createUser));
            if (validation != null) return validation;

            var result = await _authService.Register(createUser.Email, createUser.Password, createUser.UserRole);
            return Ok(ApiResponse<User>.Ok(result.Value, "User registered successfully!"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var user = await _authService.Login(request.Email, request.Password);
            var token = _tokenService.CreateToken(user.Value);

            return Ok(new { Token = token });

            
        }
    }
}
