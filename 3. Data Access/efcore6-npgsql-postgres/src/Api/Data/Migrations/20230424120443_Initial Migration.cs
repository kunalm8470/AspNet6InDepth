using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fakecompany");

            migrationBuilder.CreateTable(
                name: "departments",
                schema: "fakecompany",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP WITHOUT TIME ZONE", nullable: false, defaultValueSql: "current_timestamp AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP WITHOUT TIME ZONE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments_id", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "fakecompany",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true, computedColumnSql: "\"last_name\" || ', ' || \"first_name\"", stored: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    salary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    hire_date = table.Column<DateOnly>(type: "DATE", nullable: false),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP WITHOUT TIME ZONE", nullable: false, defaultValueSql: "current_timestamp AT TIME ZONE 'UTC'"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP WITHOUT TIME ZONE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees_id", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_departmentid",
                        column: x => x.department_id,
                        principalSchema: "fakecompany",
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "fakecompany",
                table: "departments",
                columns: new[] { "id", "name", "updated_at" },
                values: new object[,]
                {
                    { new Guid("288d6e20-44dd-42a4-871d-61afba5e6979"), "Finance", null },
                    { new Guid("53bbb954-3484-476c-a623-44d264b9cfb6"), "Board", null },
                    { new Guid("7763a7a3-fd7a-4d66-99cf-b4e03a6dbbb6"), "Legal", null },
                    { new Guid("7c198eef-df03-4f4a-a91f-4502f7dda492"), "Engineering", null },
                    { new Guid("badf4ee9-32d2-4e75-8ddf-4a6161dff48a"), "Human Resources", null },
                    { new Guid("baf283ce-3d62-4e74-aee3-f2e412d6150f"), "Admin", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_employees_department_id",
                schema: "fakecompany",
                table: "employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_email",
                schema: "fakecompany",
                table: "employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_phone",
                schema: "fakecompany",
                table: "employees",
                column: "phone",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employees",
                schema: "fakecompany");

            migrationBuilder.DropTable(
                name: "departments",
                schema: "fakecompany");
        }
    }
}
