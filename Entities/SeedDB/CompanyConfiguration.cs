using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.SeedDB
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(
                new Company
                {
                    Id = new Guid("5AC14AC5-2749-49A4-9312-4133E236D98C"),
                    Name = "IT Solution Ltd",
                    Address =  "Bangalore",
                    Country = "India"
                },
                new Company
                {
                    Id = new Guid("F961601C-8735-4CBD-8041-E9F7F43810AD"),
                    Name = "ITAdmin Solution Ltd",
                    Address = "Hyderabad",
                    Country = "India"
                }
            );
        }
    }
}
