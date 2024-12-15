using System.ComponentModel.DataAnnotations;
using HumanRegistrationSystem.Validators;

namespace HumanRegistrationSystem.DTOs.Request
{
    public record AccountRequestDto
    {
        /// <summary>
        /// Username of the account
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// Password of the account
        /// </summary>
        [Required]
        [PasswordValidator]
        public string Password { get; set; } = null!;
    }
}
