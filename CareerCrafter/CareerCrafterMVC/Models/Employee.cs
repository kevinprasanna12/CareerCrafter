using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace CareerCrafterMVC.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }



        // Navigation Property
        [ValidateNever]
        [ForeignKey("UserId")]
        public UserInfo User { get; set; }

        [ValidateNever]
        public ICollection<JobListing> JobListings { get; set; }
    }
}
