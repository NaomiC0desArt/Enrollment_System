
using UniversitySystem.Domain.Common.Base;

namespace UniversitySystem.Domain.Common.Filters
{
    public class CourseFilter : PaginationParams
    {
        public string? Title { get; set; }
        public int? Credits { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}
