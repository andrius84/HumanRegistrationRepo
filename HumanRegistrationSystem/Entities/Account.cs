using System;

namespace HumanRegistrationSystem.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        public Person? Person { get; set; }
        public Role? Role { get; set; }

        // Foreign key
        public int RoleId { get; set; } = 1;

    }
}
