namespace Api.Models;

public class Department : BaseEntity<Guid>
{
    public string Name { get; set; }

    /// <summary>
    /// Shadow property for doing .Include using EF Core 6
    /// 
    /// This property will be populated and list of employees belonging to
    /// particular department will be filled.
    /// </summary>
    public ICollection<Employee> Employees { get; } = new List<Employee>();
}
