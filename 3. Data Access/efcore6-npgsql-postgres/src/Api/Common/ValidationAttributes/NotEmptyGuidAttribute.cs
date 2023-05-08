using System.ComponentModel.DataAnnotations;

namespace Api.Common.ValidationAttributes;

/// <summary>
/// Custom data annotation validation attribute for checking empty guid
/// </summary>
public class NotEmptyGuidAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is Guid guid && guid == Guid.Empty)
        {
            return false;
        }

        return true;
    }
}
