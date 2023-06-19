namespace Api.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateTime d)
    {
        int ageInYears = DateTime.Today.Year - d.Year;

        if (d > DateTime.Today.AddYears(-ageInYears))
        {
            ageInYears--;
        }

        return ageInYears;
    }
}
