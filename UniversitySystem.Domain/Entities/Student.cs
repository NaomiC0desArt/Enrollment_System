using System.ComponentModel.DataAnnotations;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
