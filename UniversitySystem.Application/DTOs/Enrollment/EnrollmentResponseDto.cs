

namespace UniversitySystem.Application.DTOs.Enrollment
{
    public class EnrollmentResponseDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public int CourseId { get; set; }
        public string CourseTitle { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public int Grade { get; set; }
    }
}
