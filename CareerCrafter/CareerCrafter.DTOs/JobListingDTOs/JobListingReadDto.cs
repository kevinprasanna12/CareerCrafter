
namespace CareerCrafter.DTOs.JobListingDTOs
{
    public class JobListingReadDto
    {
        public int JobListingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Qualifications { get; set; }
        public decimal ? Salary { get; set; }
        public int EmployerId { get; set; }

        public string CompanyName { get; set; } 
    }
}
