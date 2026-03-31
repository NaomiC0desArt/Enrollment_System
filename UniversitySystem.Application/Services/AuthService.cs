

using Student_Course_System.Auxiliary;
using UniversitySystem.Application.Exceptions;
using UniversitySystem.Application.Repositories.Interfaces;
using UniversitySystem.Application.Services.Interfaces;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Enums;

namespace UniversitySystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<User>> Login(string email, string password)
        {
            var userInDb = await _unitOfWork.Users.GetByEmail(email);
            if (userInDb == null) 
                throw new UserFriendlyException("Invalid email or password.");

            bool isCorrect = BCrypt.Net.BCrypt.Verify(password, userInDb.PasswordHash);
            if (!isCorrect) 
                throw new UserFriendlyException("Invalid email or password.");

            return Result<User>.Success(userInDb);
        }


        public async Task<Result<User>> Register(string email, string password, Role rol)
        {

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User(email, hashPassword, rol);
            await _unitOfWork.Users.Add(user);
            await _unitOfWork.CompleteAsync();

            return Result<User>.Success(user);

        }
    }
}
