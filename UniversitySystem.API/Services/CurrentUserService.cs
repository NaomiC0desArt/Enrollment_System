using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;
using UniversitySystem.Domain.Interfaces.Common;

namespace UniversitySystem.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
