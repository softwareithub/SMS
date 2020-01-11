using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SERP.Core.Entities.Validator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CustomDateValidator : ValidationAttribute
    {
        public string MinDate { get; set; }
        public string MaxDate { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime.TryParse(value.ToString(), out DateTime validDateTime);
            if (validDateTime< Convert.ToDateTime(MinDate))
            {
                return new ValidationResult("Invalid Date, please provide valid date");
            }
            else if (validDateTime > Convert.ToDateTime(MaxDate))
            {
                return new ValidationResult("Invalid Date, please provide valid date");
            }
            return ValidationResult.Success;
        }
    }

    
}
