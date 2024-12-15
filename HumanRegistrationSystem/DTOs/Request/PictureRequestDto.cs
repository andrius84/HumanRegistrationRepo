using HumanRegistrationSystem.Validators;
using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class PictureRequestDto
    {
        [Required]
        [StringLength(100)]
        public string FileName { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string ContenType { get; set; } = null!;

        [Required]
        [FileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg" })]
        public IFormFile Data { get; set; } = null!;

        [Required]
        public Guid PersonId { get; set; }
    }
}
