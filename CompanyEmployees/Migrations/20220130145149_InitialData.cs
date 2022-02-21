using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyEmployees.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("5ac14ac5-2749-49a4-9312-4133e236d98c"), "Bangalore", "India", "IT Solution Ltd" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "Address", "Country", "Name" },
                values: new object[] { new Guid("f961601c-8735-4cbd-8041-e9f7f43810ad"), "Hyderabad", "India", "ITAdmin Solution Ltd" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("56de5ee0-2dc6-497a-84ce-5c00ae9c4241"), 20, new Guid("5ac14ac5-2749-49a4-9312-4133e236d98c"), "Charli", "Senior Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("eeb28c75-c3fb-4cc5-9742-2c9babf51ef2"), 50, new Guid("f961601c-8735-4cbd-8041-e9f7f43810ad"), "Sharh", "Senior Tester" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("56de5ee0-2dc6-497a-84ce-5c00ae9c4241"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("eeb28c75-c3fb-4cc5-9742-2c9babf51ef2"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("5ac14ac5-2749-49a4-9312-4133e236d98c"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: new Guid("f961601c-8735-4cbd-8041-e9f7f43810ad"));
        }
    }
}
