using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Models
{
    public class JobListing
    {
        public int JobListingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Qualifications { get; set; }
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }
        public ICollection<Application> Applications { get; set; }
    }
}
