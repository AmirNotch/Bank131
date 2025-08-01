using System.ComponentModel.DataAnnotations;

namespace Bank131Connector.Validations.Attributes;

public class ValidNotEmptyStringAttribute : ValidationAttribute
{
    public bool IsOptional { get; set; }
    public int MinLength { get; set; } = 3;

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
        if (stringValue.Length < MinLength)
        {
            return new ValidationResult($"The string length is less than the minimum value is {MinLength}");
        }
        return ValidationResult.Success!;
    }
}