using MoviesApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Validations
{
    public class MinYearAttribute : ValidationAttribute
    {
        public int Year { get; }

        public MinYearAttribute(int year)
        {
            Year = year;
        }
        
        public string GetErrorMessage() =>  $"No movies released before {Year}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToInt32(value) < Year)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}
