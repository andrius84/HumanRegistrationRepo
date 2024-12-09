using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class PersonRequestDto
    {
        /// <summary>
        /// First name of the person (maximum 100 characters).
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the person (maximum 100 characters).
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        /// <summary>
        /// Personal code of the person (maximum 20 characters, alphanumeric).
        /// </summary>
        [Required(ErrorMessage = "Personal code is required.")]
        [StringLength(20, ErrorMessage = "Personal code cannot exceed 20 characters.")]
        public string PersonalCode { get; set; }

        /// <summary>
        /// Phone number of the person (validated format).
        /// </summary>
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email address of the person (validated format).
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        /// <summary>
        /// Account ID of the person.
        /// </summary>
        [Required(ErrorMessage = "Account ID is required.")]
        public Guid AccountId { get; set; }
    }
}
