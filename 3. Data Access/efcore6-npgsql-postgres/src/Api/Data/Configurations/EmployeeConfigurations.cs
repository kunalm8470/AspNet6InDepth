using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees", "fakecompany");

        builder.HasKey(e => e.Id)
        .HasName("pk_employees_id");

        builder.Property(e => e.Id)
        .HasColumnName("id")
        .IsRequired()
        .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.FirstName)
        .HasColumnName("first_name")
        .IsRequired()
        .HasMaxLength(200);

        builder.Property(e => e.LastName)
        .HasColumnName("last_name")
        .IsRequired()
        .HasMaxLength(200);

        builder.Property(e => e.Email)
        .HasColumnName("email")
        .IsRequired()
        .HasMaxLength(256);

        builder.Property(e => e.Phone)
        .HasColumnName("phone")
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(e => e.Salary)
        .HasColumnName("salary")
        .IsRequired()
        .HasColumnType("DECIMAL(18, 2)");

        builder.Property(e => e.HireDate)
        .HasColumnName("hire_date")
        .IsRequired()
        .HasColumnType("DATE");

        builder.Property(e => e.DepartmentId)
        .HasColumnName("department_id")
        .IsRequired();

        builder.Property(e => e.CreatedAt)
        .HasColumnName("created_at")
        .IsRequired()
        .HasColumnType("TIMESTAMP WITHOUT TIME ZONE")
        .HasDefaultValueSql("current_timestamp AT TIME ZONE 'UTC'");

        builder.Property(propertyExpression: e => e.UpdatedAt)
        .HasColumnName("updated_at")
        .HasColumnType("TIMESTAMP WITHOUT TIME ZONE");
    }
}
