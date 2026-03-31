using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Application.Data;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Interfaces;

namespace UniversitySystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UniversityDbContext _context;

        public UserRepository(UniversityDbContext context)
        {
            _context = context;
        }
        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> ExistByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
