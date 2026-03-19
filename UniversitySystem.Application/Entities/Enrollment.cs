using System.ComponentModel.DataAnnotations;

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


    }
}
