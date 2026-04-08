using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Infrastructure.Data.Seed
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasData(
        new { Id = 1, Title = "Sistemas Operativos I", Credits = 4 },
        new { Id = 2, Title = "Programación Orientada a Objetos", Credits = 4 },
        new { Id = 3, Title = "Bases de Datos Avanzadas", Credits = 3 }
        );

            modelBuilder.Entity<User>().HasData(
            new
                {
            Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
            Email = "admin@university.do",
            PasswordHash = "$2a$11$mC8.L.R.v.y.p.v.v.v.v.v.v.v.v.v.v.v.v.v.v.v.v.v.v.v.v", // The Hash
            Role = Role.Admin,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            EmailConfirmed = true,
            FailedLoginAttempts = 0
            }
                );
        }
    }
}
