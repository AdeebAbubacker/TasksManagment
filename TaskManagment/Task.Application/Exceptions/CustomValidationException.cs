using FluentValidation.Results;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskManagment.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public List<String> ValidationErrors { get; set; } = [];

        public CustomValidationException(String errorMessage)
        {
            ValidationErrors.Add(errorMessage);
        }
        public CustomValidationException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                ValidationErrors.Add(error.ErrorMessage);
            }
        }
    }
}
