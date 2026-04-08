using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Interfaces.Repositories;
using UniversitySystem.Persistence.Data;

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
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByRefreshToken(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}
