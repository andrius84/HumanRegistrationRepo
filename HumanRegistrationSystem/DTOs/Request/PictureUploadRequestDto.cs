using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.DTOs.Request
{
    public class PictureUploadRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = null!;

        //[MaxFileSize(5 * 1024 * 1024)]
        //[AllowedExtensions([".jpeg"])]
        public IFormFile Image { get; set; } = null!;
    }
}
