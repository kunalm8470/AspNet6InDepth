using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments", "fakecompany");

        builder.HasKey(e => e.Id)
        .HasName("pk_departments_id");

        builder.Property(e => e.Id)
        .HasColumnName("id")
        .IsRequired()
        .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.Name)
        .HasColumnName("name")
        .IsRequired()
        .HasMaxLength(200);

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
