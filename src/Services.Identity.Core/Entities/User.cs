using System;
using System.Collections.Generic;
using Services.Identity.Core.Exceptions;

namespace Services.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Email { get; }
        public string Role { get; }
        public string Password { get; }
        public DateTime CreatedAt { get; }
        public IEnumerable<string> Permissions { get; }

        public User(Guid id, string email, string role, string password, DateTime createdAt, 
            IEnumerable<string> permissions = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            if (!Entities.Role.IsValid(role))
            {
                
            }
            
            Id = id;
            Email = email;
            Role = role;
            Password = password;
            CreatedAt = createdAt;
            Permissions = permissions;
        }
    }
}