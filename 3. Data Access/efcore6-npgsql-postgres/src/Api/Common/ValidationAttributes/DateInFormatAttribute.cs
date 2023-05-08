using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Api.Common.ValidationAttributes;

/// <summary>
/// Custom data annotation validation attribute for checking date is in "yyyy-MM-dd" format or not.
/// </summary>
public class DateInFormatAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is null)
        {
            // null values are handled by the Required attribute
            return true;
        }

        string validDateFormat = "yyyy-MM-dd";

        string d = (string)value;

        try
        {
            _ = DateTime.ParseExact(d, validDateFormat, CultureInfo.InvariantCulture);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
