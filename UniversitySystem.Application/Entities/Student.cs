using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Application.Entities
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public List<Enrollment> Enrollments { get; private set; } = new();

        public Student() { }
        public Student(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public List<Course> ListCourses()
        {
            return Enrollments.Select(e => e.Course).ToList();
        }

        public bool AlreadyEnroll(int courseId)
        {
            var course = Enrollments.Any(e => e.Course.Id == courseId);

            return course;
        }

        public void AddEnrollment(Enrollment enrolment)
        {
            Enrollments.Add(enrolment);
        }

    }
}
