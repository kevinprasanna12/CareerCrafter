using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCrafter.DTOs.EmployeeDTOs
{
    public class EmployeeUpdateDto
    {

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string ContactEmail { get; set; }
    }
}
