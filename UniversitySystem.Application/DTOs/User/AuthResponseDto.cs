

namespace UniversitySystem.Application.DTOs.User
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public bool MustChangePassword { get; set; }
    }
}
