using Api.Models;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.Data;

/// <summary>
/// Reperesentation of the database
/// </summary>
public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define 1 Employee : 1 Department
        // 1 Department : N Employees
        modelBuilder.Entity<Employee>()
        .HasOne(e => e.Department)
        .WithMany(d => d.Employees)
        .HasForeignKey(e => e.DepartmentId)
        .HasConstraintName("fk_employees_departmentid")
        .OnDelete(DeleteBehavior.Cascade); // ON DELETE CASCADE

        // Add unique index on the email column
        modelBuilder.Entity<Employee>()
        .HasIndex(e => e.Email)
        .IsUnique();

        // Add unique index on the phone column
        modelBuilder.Entity<Employee>()
        .HasIndex(e => e.Phone)
        .IsUnique();

        // Generate computed column display_name -> "last_name, first_name"
        modelBuilder.Entity<Employee>()
        .Property(e => e.DisplayName)
        .HasComputedColumnSql(@"""last_name"" || ', ' || ""first_name""", stored: true);

        // Seed the department initially
        modelBuilder.Entity<Department>()
        .HasData(new Department[]
        {
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Board"
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Human Resources"
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Finance"
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Legal"
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Admin"
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Engineering"
            }
        });

        // Apply the configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Wrap common exceptions using EntityFrameworkCore.Exceptions.PostgreSQL
        optionsBuilder.UseExceptionProcessor();

        base.OnConfiguring(optionsBuilder);
    }
}
