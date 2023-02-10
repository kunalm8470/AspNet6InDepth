using Api.Models;
using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Api.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.FirstName)
        .Length(2, 200)
        .WithMessage("{PropertyName} should be between 2 and 200 and you provided {PropertyValue}")
        .Must(x =>
            !string.IsNullOrEmpty(x)
            && !string.IsNullOrWhiteSpace(x))
        .WithMessage("First Name cannot be empty string or a whitespace string or a null string");


        RuleFor(x => x.LastName)
        .Length(1, 200)
        .WithMessage("{PropertyName} should be between 1 and 200 and you provided {PropertyValue}")
        .Must(x =>
            !string.IsNullOrEmpty(x)
            && !string.IsNullOrWhiteSpace(x))
        .WithMessage("{PropertyName} cannot be empty string or a whitespace string or a null string");

        RuleFor(x => x.DateOfBirth)
        .Must(x => x > DateTime.UtcNow)
        .WithMessage("Date of birth cannot be a previous date");

        RuleFor(x => x.DateOfJoining)
        .Must(x => x > DateTime.UtcNow)
        .WithMessage("Date of joining cannot be a previous date");

        RuleFor(x => x.Code)
        .Must(x => Regex.IsMatch(x, "EMP\\d{5}"))
        .WithMessage("Invalid Employee Code")
        .Must(x =>
            !string.IsNullOrEmpty(x)
            && !string.IsNullOrWhiteSpace(x))
        .WithMessage("{PropertyName} cannot be empty string or a whitespace string or a null string");


        RuleFor(x => x.Gender)
        .IsInEnum<Employee, Gender>()
        .WithMessage("Invalid Gender");

        RuleFor(x => x.Designation)
        .Length(1, 200)
        .WithMessage("{PropertyName} should be between 1 and 200 and you provided {PropertyValue}")
        .Must(x =>
            !string.IsNullOrEmpty(x)
            && !string.IsNullOrWhiteSpace(x))
        .WithMessage("{PropertyName} cannot be empty string or a whitespace string or a null string");


        RuleFor(x => x.Email)
        .Must(IsValidEmail)
        .WithMessage("Invalid email, you passed {PropertyValue}");
    }

    /// <summary>
    /// Helper method to validate email - Source MSDN https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            // Normalize the domain
            email = Regex.Replace(
                input: email,
                pattern: @"(@)(.+)$",
                evaluator: DomainMapper,
                options: RegexOptions.None,
                matchTimeout: TimeSpan.FromMilliseconds(200)
            );

            // Examines the domain part of the email and normalizes it.
            static string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                IdnMapping idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(
                input: email,
                pattern: @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                options: RegexOptions.IgnoreCase,
                matchTimeout: TimeSpan.FromMilliseconds(250)
            );
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
