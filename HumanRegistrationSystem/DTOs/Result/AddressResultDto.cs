namespace HumanRegistrationSystem.DTOs.Result
{
    public class AddressResultDto
    {
        /// <summary>
        /// City of the address
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Street of the address
        /// </summary>
        public string? Street { get; set; }

        /// <summary>
        /// HouseNumber of the address
        /// </summary>
        public string? HouseNumber { get; set; }

        /// <summary>
        /// ApartamentNumber number of the address
        /// </summary>
        public string? ApartmentNumber { get; set; }
    }
}
