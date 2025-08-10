using System;
using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public MinimumAgeAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Check if DOB is null or invalid
            if (value == null || !(value is DateTime dob))
            {
                return new ValidationResult("Invalid date of birth.");
            }

            // Calculate age based on today's date
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;

            // Validate age range
            if (age < _min || age > _max)
            {
                return new ValidationResult($"Age must be between {_min} and {_max}.");
            }

            return ValidationResult.Success;
        }
    }
}
