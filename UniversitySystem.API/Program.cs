using Microsoft.EntityFrameworkCore;
using Student_Course_System.Services;
using System.Text.Json.Serialization;
using UniversitySystem.API.Extensions;
using UniversitySystem.API.Middleware;
using UniversitySystem.Application.Data;
using UniversitySystem.Application.Repositories;
using UniversitySystem.Application.Repositories.Interfaces;
using UniversitySystem.Application.Services;
using UniversitySystem.Application.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
