using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Employee
    {
        [Column("EmployeeId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Employee Name is a required field.")]
        [MaxLength(60, ErrorMessage = "Max length of Employee Name is 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Employee Age is a required field.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(60, ErrorMessage = "Max length of Employee Position is 60 characters.")]
        public string Position { get; set; }
        
        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
