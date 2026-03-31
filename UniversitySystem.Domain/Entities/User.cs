using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    }
}
