using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.DTOs.User
{
    public class CreateUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role UserRole { get; set; }
    }
}
