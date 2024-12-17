using System.ComponentModel.DataAnnotations;
using HumanRegistrationSystem.Validators;

namespace HumanRegistrationSystem.DTOs.Request
{
    /// <summary>
    /// User account login request
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Username of the account
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        /// <summary>
        /// Password of the account
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
