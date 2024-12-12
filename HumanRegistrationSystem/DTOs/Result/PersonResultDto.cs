namespace HumanRegistrationSystem.DTOs.Result
{
    public class PersonResultDto
    {
        /// <summary>
        /// Id of the Person
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the Person
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of the Person
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Personal code for the Person
        /// </summary>
        public string? PersonalCode { get; set; }

        /// <summary>
        /// Phone number of the Person
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email of the Person
        /// </summary>
        public string? Email { get; set; }

    }
}
