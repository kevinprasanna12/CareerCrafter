using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCrafter.DTOs.EmployeeDTOs
{
    public class EmployeeReadDto
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
    }
}
