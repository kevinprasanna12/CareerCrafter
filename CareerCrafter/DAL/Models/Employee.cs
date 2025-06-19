using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Employee
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public ICollection<JobListing> JobListings { get; set; }
    }
}
