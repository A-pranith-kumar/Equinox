using System;
using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dob)
            {
                var today = DateTime.Today;
                var age = today.Year - dob.Year;

                if (dob > today.AddYears(-age))
                    age--;

                if (age >= _minimumAge)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ErrorMessage ?? $"Minimum age is {_minimumAge}.");
            }

            return new ValidationResult("Invalid date format.");
        }
    }
}
