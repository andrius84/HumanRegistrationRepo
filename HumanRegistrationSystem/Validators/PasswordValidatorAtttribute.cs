using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PasswordValidatorAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 4;
        public int MaximumLength { get; set; } = 40;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Neįvestas slaptažodis");
            }

            var password = value.ToString();

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Neįvestas slaptažodis");
            }

            if (password.Length < MinimumLength)
            {
                return new ValidationResult($"Slaptažodis turi būti mažiausiai {MinimumLength}");
            }

            if (password.Length > MaximumLength)
            {
                return new ValidationResult($"Slaptažodis turi būti neilgesnis nei {MaximumLength}");
            }

            if (RequireUppercase && !password.Any(char.IsUpper))
            {
                return new ValidationResult("Slaptažodyje turi būti didžiųjų raidžių");
            }

            if (RequireLowercase && !password.Any(char.IsLower))
            {
                return new ValidationResult("Slaptažodyje turi b9ti mažųjų raidžių");
            }

            return ValidationResult.Success;
        }
    }
}
