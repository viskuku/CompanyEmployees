﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Company
    {
        [Column("CompanyId")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Company Name is a required field.")]
        [MaxLength(60, ErrorMessage ="Max length of Name is 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company Address is a required field.")]
        [MaxLength(60, ErrorMessage = "Max length of Address is 60 characters.")]
        public string Address { get; set; }
        public string Country { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
