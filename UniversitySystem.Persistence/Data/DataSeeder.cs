using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Persistence.Data;

namespace UniversitySystem.Application.Data
{
    public class DataSeeder
    {
        public static void Seed(UniversityDbContext data)
        {
            // Verificamos si ya hay datos para no duplicarlos cada vez que se reinicie
            if (data.Students.Any() || data.Courses.Any()) return;

            // 1. Sembrar Estudiantes
            var s1 = new Student("Alice Johnson", "alice@university.com");
            var s2 = new Student("Bob Smith", "bob@university.com");
            var s3 = new Student("Charlie Brown", "charlie@university.com");

            data.Students.AddRange(new[] { s1, s2, s3 });

            // 2. Sembrar Cursos
            var c1 = new Course("Introducción a C#", 5);
            var c2 = new Course("Arquitectura de Software", 4);
            var c3 = new Course("Bases de Datos", 3);

            data.Courses.AddRange(new[] { c1, c2, c3 });

            // 3. Sembrar Matrículas (Enrollments)
            // Aquí simulamos que Alice y Bob se inscriben en C#
            var e1 = new Enrollment(s1, c1);
            e1.setGrade(95);
            var e2 = new Enrollment(s2, c1);
            e2.setGrade(88);
            var e3 = new Enrollment(s1, c2);
            e3.setGrade(92);

            data.Enrollments.Add(e1);
            data.Enrollments.Add(e2);
            data.Enrollments.Add(e3);

            // Importante: Como tu servicio EnrollmentService hace esto manualmente, 
            // aquí en el seeder también debemos "conectar" los cables:
            s1.AddEnrollment(e1);
            s1.AddEnrollment(e3);
            s2.AddEnrollment(e2);

            c1.AddEnrollment(e1);
            c1.AddEnrollment(e2);
            c2.AddEnrollment(e3);
        }
    }
}
