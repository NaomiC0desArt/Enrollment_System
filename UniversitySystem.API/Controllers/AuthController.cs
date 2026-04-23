using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.API.Controllers.BaseController;
using UniversitySystem.Application.Auxiliary;
using UniversitySystem.Application.DTOs.ApiResponse;
using UniversitySystem.Application.DTOs.User;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Interfaces.Common;

namespace UniversitySystem.API.Controllers
{

    [Route("api/[controller]")]
    [Tags("Authentication")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
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

        #region 
        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <remarks>Creates a new user identity in the system with a specified role. This endpoint triggers an email confirmation flow if enabled. It validates the input against password complexity and email format rules.</remarks>
        /// <param name="createUser"></param>
        /// <returns></returns>
        #endregion
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUser createUser)
        {
            var validation = HandleValidation(await _validator.ValidateAsync(createUser));
            if (validation != null) return validation;

            var result = await _authService.Register(createUser.Email, createUser.Password, createUser.UserRole);
            return HandleResult(result);
        }

        #region
        /// <summary>
        /// Authenticates a user and returns access and refresh tokens.
        /// </summary>
        /// <remarks>
        /// The access token expires in 15 minutes.
        /// Use the refresh token endpoint to get a new one without logging in again.
        /// </remarks>
        /// <response code="200">Login successful, tokens returned</response>
        /// <response code="401">Invalid credentials or account not confirmed</response>
        /// <response code="403">Account is temporarily locked</response>
#endregion
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var result = await _authService.Login(request.Email, request.Password);

            if (result.IsSuccess)
            {
                return BuildAuthResponse(result.Value);
            }

            return HandleResult(result);
        }

        #region
        /// <summary>
        /// Verifies a user's email address.
        /// </summary>
        /// <remarks>Validates a unique token sent to the user's email. This is a critical step to activate the account and permit future logins.</remarks>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <response code="200">Your email has been confirmed!</response>
        /// <returns></returns>
#endregion
        [AllowAnonymous]
        [HttpGet("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await _authService.ConfirmEmail(email, token);
            return HandleResult(result);
        }
        #region
        /// <summary>
        /// Initiates the password recovery process.
        /// </summary>
        /// <remarks>Accepts a registered email address and, if the user exists, generates a secure password reset token sent via email.</remarks>
        /// <param name="email"></param>
        /// <response code="200">An email has been send.</response>
        /// <returns></returns>
        #endregion
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var result = await _authService.ForgotPassword(request.Email);
            return HandleResult(result);
        }
        #region
        /// <summary>
        /// Updates a user's password using a reset token.
        /// </summary>
        /// <remarks>Finalizes the recovery process. It validates the provided reset token and updates the user's password to the new value provided in the request body.</remarks>
        /// <param name="request"></param>
        /// <response code="200">The password has been changed.</response>
        /// <returns></returns>
        #endregion
        [AllowAnonymous]
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            var validation = HandleValidation(await _resetPasswordValidator.ValidateAsync(request));
            if (validation != null) return validation;

            var result = await _authService.ResetPassword(request);
            return HandleResult(result);
        }
        #region
        /// <summary>
        /// Issues a new JWT access token.
        /// </summary>
        /// <remarks>Exchange a valid, non-expired Refresh Token for a brand new Access Token (JWT). This allows users to stay logged in without re-entering credentials once their short-lived access token expires.</remarks>
        /// <param name="refreshToken"></param>
        /// <response code="200">The token has been refreshed</response>
        /// <returns></returns>
        #endregion
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(ApiResponse<Object>.Fail("No refresh token provided."));

            var result = await _authService.RefreshToken(refreshToken);

            if (result.IsSuccess)
            {
                return BuildAuthResponse(result.Value);
            }
                

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.Logout(refreshToken);
            }

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return NoContent();



        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        private IActionResult BuildAuthResponse(AuthInternalResult internalResult)
        {
            
                SetRefreshTokenCookie(internalResult.RefreshToken);

                var publicResponse = new AuthResponseDto
                {
                    AccessToken = internalResult.AccessToken,
                    MustChangePassword = internalResult.MustChangePassword
                };

                return Ok(ApiResponse<AuthResponseDto>.Ok(publicResponse));
        }
    }
}
