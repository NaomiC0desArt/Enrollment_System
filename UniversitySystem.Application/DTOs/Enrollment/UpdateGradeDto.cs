using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Application.DTOs.Enrollment
{
    public class UpdateGradeDto
    {
        public int EnrollmentId { get; set; }
        public int NewGrade { get; set; }

    }
}
