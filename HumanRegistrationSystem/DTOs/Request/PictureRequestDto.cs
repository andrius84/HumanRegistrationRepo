using HumanRegistrationSystem.Validators;
using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class PictureRequestDto
    {
        /// <summary>
        /// Name of the file.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FileName { get; set; } = null!;

        /// <summary>
        /// Content type of the file.
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string ContentType { get; set; } = null!;

        /// <summary>
        /// Data of the file.
        /// </summary>
        [Required]
        [FileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new[] { ".jpg", ".jpeg" })]
        public IFormFile Data { get; set; } = null!;

        /// <summary>
        /// Person ID of the picture.
        /// </summary>
        [Required]
        public Guid PersonId { get; set; }
    }
}
