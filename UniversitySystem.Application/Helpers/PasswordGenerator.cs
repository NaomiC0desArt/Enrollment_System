using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.Helpers
{
    public class PasswordGenerator
    {
        public string GeneratePassword()
        {
            return $"Student{DateTime.Now.Year}!";
        }
    }
}
