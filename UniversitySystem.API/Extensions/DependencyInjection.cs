using Student_Course_System.Services;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Application.Services;
using UniversitySystem.Application.Data;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Repositories.Interfaces;
using UniversitySystem.Application.Repositories;
using UniversitySystem.Infrastructure.UnitOfWork;

namespace UniversitySystem.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UniversityDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
