namespace Api.Models.Dto.Responses;

public record MappedEmployee(Guid Id, string FirstName, string LastName, string DisplayName, string Email, string Phone, decimal Salary, string HireDate, MappedDepartment Department);

