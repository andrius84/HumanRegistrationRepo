using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class PersonRequestDto
    {
        /// <summary>
        /// First name of the Person
        /// </summary>
        [Required]
        [StringLength(100)]
        //[TodoTypeValidator]
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of the Person
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? LastName { get; set; }

        /// <summary>
        /// Personal code for the Person
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? PersonalCode { get; set; }

        /// <summary>
        /// Phone number of the Person
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email of the Person
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Email { get; set; }
    }
}
