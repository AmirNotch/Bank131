using System.ComponentModel.DataAnnotations;

namespace Bank131Connector.Validations.Attributes;

public class ValidSessionAttribute : ValidationAttribute
{
    public bool IsOptional { get; set; }
    private string ReadyToConfirm { get; set; } = "ready_to_confirm";

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (IsOptional && value == null)
        {
            return ValidationResult.Success!;
        }
        
        if (value == null)
        {
            return new ValidationResult("Type session is required");
        }

        if (value is not string TypeSession)
        {
            return new ValidationResult("Type session must be a string");
        }
        
        if (value.ToString()!.Equals(ReadyToConfirm))
        {
            return new ValidationResult($"Type session must be {ReadyToConfirm}");
        }
        
        return ValidationResult.Success!;
    }
}