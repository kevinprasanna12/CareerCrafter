using CareerCrafter.DTOs.ResumeDTOs;

namespace CareerCrafter.DTOs.ApplicationDTOs
{
    public class EmployerApplicationReadDto
    {
        public int ApplicationId { get; set; }
        public int JobListingId { get; set; }
        public string JobTitle { get; set; }
        public int JobSeekerId { get; set; }
        public string JobSeekerName { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }

        public ResumeReadDto Resume { get; set; }
        
    }
}
