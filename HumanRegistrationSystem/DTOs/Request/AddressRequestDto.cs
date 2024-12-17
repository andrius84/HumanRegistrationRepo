using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class AddressRequestDto
    {
        /// <summary>
        /// City of the address
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Street of the address
        /// </summary>
        [Required]
        public string Street { get; set; }

        /// <summary>
        /// HouseNumber of the address
        /// </summary>
        [Required]
        public string HouseNumber { get; set; }

        /// <summary>
        /// ApartamentNumber number of the address
        /// </summary>
        public string? ApartmentNumber  { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [Required]
        public Guid PersonId { get; set; }
    }
}
