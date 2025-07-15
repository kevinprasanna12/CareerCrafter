using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Models
{
    public class JobListing
    {
        [Key]
        public int JobListingId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [StringLength(500)]
        public string Qualifications { get; set; }


        public decimal? Salary { get; set; }


        [Required]
        public int EmployerId { get; set; }

        [ForeignKey("EmployerId")]
        public Employee Employer { get; set; }

        public ICollection<Application> Applications { get; set; }
    }
}
