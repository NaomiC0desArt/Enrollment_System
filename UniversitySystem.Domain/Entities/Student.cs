using System.ComponentModel.DataAnnotations;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Entities
{
    public class Student : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<Enrollment> Enrollments { get; private set; } = new();

        public Student() { }
        public Student(string name, string email, Guid userId)
        {
            Name = name;
            Email = email;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
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
