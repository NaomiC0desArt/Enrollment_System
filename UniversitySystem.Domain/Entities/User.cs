using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using UniversitySystem.Domain.Enums;
using UniversitySystem.Domain.Interfaces;

namespace UniversitySystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { set; get; }
        [EmailAddress]
        [MaxLength(255)]
        public string Email { set; get; }
        [MaxLength(255)]
        public string PasswordHash { get; private set; }

        public Role Role { get; set; }

        public bool MustChangePassword { get; set; } = false;

        public bool EmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiry { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }


        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        public User()
        {
        }

        public User(string email, string passwordHash, Role rol)
        {
            Id = Guid.NewGuid();
            this.Email = email;
            this.PasswordHash = passwordHash;
            Role = rol;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        //remember to always use DateTime.UtcNow, in case the server moves.
        //
        public void RevokeRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiry = null;
        }

        public void ClearPasswordResetToken()
        {
            PasswordResetToken = null;
            PasswordResetTokenExpiry = null;
        }

        public void ClearConfirmationToken()
        {
            EmailConfirmationToken = null;
            EmailConfirmationTokenExpiry = null; 
        }

        public void ChangePassword(string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new ArgumentException("Password hash cannot be empty", nameof(hashedPassword));
            }

            PasswordHash = hashedPassword;
        }

    }
}
