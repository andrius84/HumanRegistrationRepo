using System.ComponentModel.DataAnnotations.Schema;

namespace HumanRegistrationSystem.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string HouseNumber { get; set; } = null!;
        public string? ApartmentNumber { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; } = null!;
    }
}
