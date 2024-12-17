using System.ComponentModel.DataAnnotations;
using HumanRegistrationSystem.Validators;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class PersonRequestDto
    {
        /// <summary>
        /// Account ID of the person.
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        /// First name of the person (maximum 50 characters).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the person (maximum 50 characters).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Personal code of the person (exactly 11 digits).
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Personal code must be exactly 11 digits.")]
        public string PersonalCode { get; set; }

        /// <summary>
        /// Phone number of the person (validated format).
        /// </summary>
        [Required]
        
        [StringLength(15, MinimumLength = 9)]
        [RegularExpression(@"^\+?[0-9]{7,15}$")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email address of the person (validated format).
        /// </summary>
        [Required]
        [StringLength(100)]
        [EmailDomainValidator]
        public string Email { get; set; }

    }
}
