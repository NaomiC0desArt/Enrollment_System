using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Auxiliary;

namespace UniversitySystem.Application.Interfaces.Auth
{
    public interface IEmailService
    {
        Task<Result> SendConfirmationEmailAsync(string email, string token, string rol, string? password);
        Task<Result> SendPasswordResetEmailAsync(string email, string token);
    }
}
