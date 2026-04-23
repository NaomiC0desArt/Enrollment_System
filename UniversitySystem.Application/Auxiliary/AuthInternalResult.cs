using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.Auxiliary
{
    public class AuthInternalResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool MustChangePassword { get; set; }
    }
}
