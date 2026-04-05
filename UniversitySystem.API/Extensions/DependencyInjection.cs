
#region usings
using Student_Course_System.Services;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Application.Services;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using UniversitySystem.API.Services;
using UniversitySystem.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.DTOs.ApiResponse;
using UniversitySystem.Domain.Interfaces.Common;
using UniversitySystem.Domain.Interfaces.Repositories;
using UniversitySystem.Persistence.Data;
using UniversitySystem.Persistence.UnitOfWork;
#endregion

namespace UniversitySystem.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateStudentValidator>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    // Extract the errors from the ModelState
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    // Wrap them in your ApiResponse structure
                    var response = ApiResponse<object>.Fail("Validation failed", errors);

                    return new BadRequestObjectResult(response);
                };
            });


            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UniversityDbContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //register tokenservice implementation
            services.AddScoped<ITokenService, TokenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"])),
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "University API", Version = "v1" });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token (Format: 'Bearer {token}')",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            return services;
    
        }
    }
}
