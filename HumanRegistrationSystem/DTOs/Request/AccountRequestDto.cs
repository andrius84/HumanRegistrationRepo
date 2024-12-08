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
        public string? UserName { get; set; }

        /// <summary>
        /// Password of the account
        /// </summary>
        [PasswordValidator]
        public string? Password { get; set; }

        ///// <summary>
        ///// Role of the account
        ///// </summary>
        //[RoleValidator]
        //public string? RoleId { get; set; }
    }
}
