using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Common.Base;

namespace UniversitySystem.Domain.Common.Filters
{
    public class EnrollmentFilter : PaginationParams
    {
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }

        public int? MinGrade { get; set; }
        public int? MaxGrade { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
