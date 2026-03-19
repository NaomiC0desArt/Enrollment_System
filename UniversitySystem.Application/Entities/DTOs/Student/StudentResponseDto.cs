using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.Entities.DTOs.Student
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }

        public List<string> EnrolledCourseTitles { get; set; } = new();
    }
}
