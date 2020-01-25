using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Helper
{
    public class CustomDateValidator : ValidationAttribute
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public string MinDateErrorMessage { get; set; }
        public string MaxDateErrorMessage { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime.TryParse(value.ToString(), out DateTime validDateTime);
            if (validDateTime < MinDate)
            {
                return new ValidationResult(MinDateErrorMessage);
            }
            else if (validDateTime > MaxDate)
            {
                return new ValidationResult(MaxDateErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
