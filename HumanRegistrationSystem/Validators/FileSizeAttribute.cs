using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.Validators
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;

        public FileSizeAttribute(long maxFileSizeInBytes)
        {
            _maxFileSize = maxFileSizeInBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return ValidationResult.Success;

            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"File size cannot exceed {_maxFileSize / 1024.0 / 1024.0:F2} MB.");
            }

            return ValidationResult.Success;
        }
    }
}
