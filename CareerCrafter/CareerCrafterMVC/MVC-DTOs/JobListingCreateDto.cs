using System.ComponentModel.DataAnnotations;

namespace CareerCrafterMVC.Models
{
    public class JobListingCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal ? Salary { get; set; }
        [Required]
        public string Qualifications { get; set; }
        public int EmployerId { get; set; }
    }
}
