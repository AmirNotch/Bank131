using System.ComponentModel.DataAnnotations;

namespace Bank131Connector.Validations.Attributes;

public class ValidCardNumberAttribute : ValidationAttribute
{
    public bool IsOptional { get; set; }
    public bool UseLuhnCheck { get; set; } = true; // Проверять алгоритмом Луна по умолчанию

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (IsOptional && value == null)
        {
            return ValidationResult.Success!;
        }
        
        if (value == null)
        {
            return new ValidationResult("Card number is required");
        }

        if (value is not string cardNumber)
        {
            return new ValidationResult("Card number must be a string");
        }

        // Удаление всех пробелов и дефисов
        var cleanedNumber = cardNumber.Replace(" ", "").Replace("-", "");
        
        if (cleanedNumber.Length != 16)
        {
            return new ValidationResult("Card number must be 16 digits long");
        }
        
        if (!cleanedNumber.All(char.IsDigit))
        {
            return new ValidationResult("Card number must contain only digits");
        }

        // // Проверка алгоритмом Луна (опционально)
        // if (UseLuhnCheck && !IsValidLuhn(cleanedNumber))
        // {
        //     return new ValidationResult("Invalid card number (Luhn check failed)");
        // }

        return ValidationResult.Success!;
    }

    // Алгоритм Луна для проверки валидности номера карты
    private static bool IsValidLuhn(string number)
    {
        int sum = 0;
        bool alternate = false;
        
        for (int i = number.Length - 1; i >= 0; i--)
        {
            var digit = number[i] - '0';
            
            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit = (digit % 10) + 1;
                }
            }
            
            sum += digit;
            alternate = !alternate;
        }
        
        return (sum % 10) == 0;
    }
}
