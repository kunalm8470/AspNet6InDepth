using System.ComponentModel.DataAnnotations;

namespace Api.ValidationAttributes;

/// <summary>
/// Custom validation attribute made by inherting from ValidationAttribute class
/// and overriding the IsValid method of the base class.
/// </summary>
[AttributeUsage(validOn: AttributeTargets.Property, AllowMultiple = false)]
public class PreviousDateValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return value is DateTime dt && dt.Date <= DateTime.Today.Date;
    }
}
