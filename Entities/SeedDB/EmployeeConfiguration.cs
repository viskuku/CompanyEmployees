using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.SeedDB
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(
                new Employee
                {
                    Id = new Guid("56DE5EE0-2DC6-497A-84CE-5C00AE9C4241"),
                    Age = 20,
                    Name = "Charli",
                    Position = "Senior Developer",
                    CompanyId = new Guid("5AC14AC5-2749-49A4-9312-4133E236D98C")
                },
                new Employee
                {
                    Id = new Guid("EEB28C75-C3FB-4CC5-9742-2C9BABF51EF2"),
                    Age = 50,
                    Name = "Sharh",
                    Position = "Senior Tester",
                    CompanyId = new Guid("F961601C-8735-4CBD-8041-E9F7F43810AD")
                }
              );
        }
    }
}
