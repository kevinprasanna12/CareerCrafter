using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Employee
    {
        [Key]
        public int EmployerId { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string ContactEmail { get; set; }

        public ICollection<JobListing> JobListings { get; set; }

        [Required]
        public int UserId { get; set; }  

        [ForeignKey("UserId")]
        public UserInfo User { get; set; }
    }
}

