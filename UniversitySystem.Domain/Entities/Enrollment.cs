using System.ComponentModel.DataAnnotations;
using UniversitySystem.Domain.Entities;

namespace UniversitySystem.Application.Entities
{

    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        //navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }


        public DateTime EnrollmentDate { get; set; }
        [Range(0, 100)]
        public int Grade { get; private set; }

        public Enrollment() { }
        public Enrollment(Student student, Course course)
        {
            Student = student;
            Course = course;
            StudentId = student.Id; 
            CourseId = course.Id;
            EnrollmentDate = DateTime.Now;
        }

        public void setGrade(int grade)
        {
            if (grade >= 0 && grade <= 100)
            {
                Grade = grade;
            }
        }

        public void UpdateGrade(int newGrade)
        {
            if (newGrade < 0 || newGrade > 100)
                throw new Exception("Grade must be between 0 and 100.");

            Grade = newGrade;
        }

    }
}
