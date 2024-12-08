﻿using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.Attributes
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public FileExtensionAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return ValidationResult.Success;

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_extensions.Contains(fileExtension))
            {
                return new ValidationResult($"This file type is not allowed. Allowed extensions: {string.Join(", ", _extensions)}.");
            }

            return ValidationResult.Success;
        }
    }
}
