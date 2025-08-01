using System.ComponentModel.DataAnnotations;

namespace Bank131Connector.Validations.Attributes;

public class ValidFullNameAttribute : ValidationAttribute
{
    public bool IsOptional { get; set; }
    public int MinLength { get; set; } = 2;
    public int MaxLength { get; set; } = 3;
    public bool RequireThreeWords { get; set; }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (IsOptional && value == null)
        {
            return ValidationResult.Success!;
        }
        
        if (value == null || value is not string stringValue)
        {
            return new ValidationResult("Value is not a string");
        }
        
        if (string.IsNullOrEmpty(stringValue))
        {
            return new ValidationResult("Value shall not be empty");
        }
        
        // Новая проверка на минимум 2 слова и максимум 3 слова
        if (RequireThreeWords)
        {
            var words = stringValue.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < MinLength || words.Length > MaxLength)
            {
                return new ValidationResult("The string must consist of exactly three words");
            }
        }
        
        return ValidationResult.Success!;
    }
}