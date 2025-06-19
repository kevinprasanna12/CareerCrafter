using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }

        public int JobListingId { get; set; }
        public JobListing JobListing { get; set; }

        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }
    }
}
