namespace Api.Models;

public class Employee
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; set; }

    public string Code { get; set; }

    public Gender Gender { get; set; }

    public string Designation { get; set; }

    public string Email { get; set; }
}

public enum Gender
{
    Male = 0,
    Female,
    Others
}
