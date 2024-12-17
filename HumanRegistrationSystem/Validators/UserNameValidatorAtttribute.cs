using System.ComponentModel.DataAnnotations;

namespace HumanRegistrationSystem.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UserNameValidatorAttribute : ValidationAttribute
    {
        public int MinimumLength { get; set; } = 3;
        public int MaximumLength { get; set; } = 20;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Neįvestas vartotojo vardas");
            }

            var password = value.ToString();

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Neįvestas vartotojo vardas");
            }

            if (password.Length < MinimumLength)
            {
                return new ValidationResult($"Vartotojo vardas turi būti mažiausiai iš {MinimumLength} simbolių");
            }

            return ValidationResult.Success;
        }
    }
}
