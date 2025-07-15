using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCrafterMVC.Models
{
    public class JobListing
    {
        [Key]
        public int JobListingId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ? Salary { get; set; }

        [Required]
        public string Qualifications { get; set; }

        [Required]
        public int EmployerId { get; set; }

        //public DateTime PostedDate { get; set; }



        [ValidateNever]
        // Navigation Property
        [ForeignKey("EmployerId")]
        public Employee Employer { get; set; }
    }
}
