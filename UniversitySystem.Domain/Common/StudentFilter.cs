using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.DTOs.Common
{
    public class StudentFilter: PaginationParams
    {
        public string? Name { get; set; }
        public string? EmailDomain { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
