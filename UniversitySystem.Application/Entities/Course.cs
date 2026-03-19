using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Application.Entities
{
    public class Course
    {
        public int Id { get; private set; }
        [Required]
        public string Title { get; private set; }
        [Range(0, 5)]
        public int Credits { get; private set; }

        public List<Enrollment> Enrollments { get; set; } = new();

        public Course() { }
        public Course(string title, int credits)
        {
            Title = title;
            Credits = credits;
        }

        public List<Student> ListEnrolledStudents()
        {
            return Enrollments.Select(e => e.Student).ToList();
        }

        public void AddEnrollment(Enrollment enrolment)
        {
            Enrollments.Add(enrolment);
        }

    }
}
