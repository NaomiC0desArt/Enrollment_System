using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Entities;

namespace UniversitySystem.Application.Data
{
    public class UniversityDbContext : DbContext
    {
        public DbSet<Student> Students { get; set;}
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
        {

        }
    }
}
