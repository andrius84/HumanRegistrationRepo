using HumanRegistrationSystem.Attributes;
using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class UploadImageDto
    {
        [Required]
        [FileExtension(new[] { ".jpg", ".jpeg", ".png" })]
        [FileSize(5 * 1024 * 1024)]
        public IFormFile File { get; set; } = null!;
    }
}
