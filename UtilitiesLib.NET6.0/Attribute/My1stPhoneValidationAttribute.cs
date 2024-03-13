using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesLib.NET6._0.Attribute;

public class My1stPhoneValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        
        if(int.TryParse(value.ToString(), out int _))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage = "Not a valid phone number, use digits only.");
    }
}
