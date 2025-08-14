using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Equinox.Models.Validation
{
    /// <summary>
    /// Validates that a date corresponds to an age within [min,max] (inclusive).
    /// Emits unobtrusive validation attributes so client-side JS can validate too.
    /// </summary>
    public class MinimumAgeAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _min;
        private readonly int _max;

        public MinimumAgeAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        // NOTE: match base signature (nullable)
        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is DateTime dob)
            {
                var today = DateTime.Today;
                var age = today.Year - dob.Year;
                if (dob.Date > today.AddYears(-age)) age--;

                if (age >= _min && age <= _max)
                    return ValidationResult.Success;   // no null-forgiving needed
            }

            var display = ctx.DisplayName ?? "Date";
            return new ValidationResult(ErrorMessage ?? $"{display} must make the age between {_min} and {_max}.");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (!context.Attributes.ContainsKey("data-val"))
                context.Attributes.Add("data-val", "true");

            // message + params used by the JS adapter
            context.Attributes["data-val-minimumage"] = ErrorMessage ?? $"Age must be between {_min} and {_max}.";
            context.Attributes["data-val-minimumage-min"] = _min.ToString();
            context.Attributes["data-val-minimumage-max"] = _max.ToString();
        }
    }
}
