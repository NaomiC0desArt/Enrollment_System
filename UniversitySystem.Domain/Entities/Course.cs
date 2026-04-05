using System.ComponentModel.DataAnnotations;
using UniversitySystem.Application.Entities;

namespace UniversitySystem.Domain.Entities
{
    public class Course
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
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
