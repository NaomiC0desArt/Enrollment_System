using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.Entities.DTOs.Course
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Credit { get; set; }

        public List<string>? StudentsName { get; set; }
    }
}
