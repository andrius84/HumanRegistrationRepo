using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace HumanRegistrationSystem.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PersonalCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        // Foreign key
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Address? Address { get; set; }
        public ProfilePicture? ProfilePicture { get; set; }

    }
}
