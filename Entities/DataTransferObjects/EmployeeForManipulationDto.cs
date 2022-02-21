using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class EmployeeForManipulationDto
    {

        [Required(ErrorMessage = "Employee Name is required field")]
        [MaxLength(20, ErrorMessage = "Employee Name Max length is 20 character")]
        public string Name { get; set; }

        [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Position is required field")]
        [MaxLength(20, ErrorMessage = "Position Max length is 20 character")]
        public string Position { get; set; }
    }
}
