using CareerCrafter.DTOs.ResumeDTOs;

namespace CareerCrafter.DTOs.ApplicationDTOs
{

    public class ApplicationReadDto
    {
        public int ApplicationId { get; set; }
        public int JobSeekerId { get; set; }
        public int JobListingId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Status { get; set; }

        public string JobTitle { get; set; }
        public string companyName { get; set; }

        public ResumeReadDto Resume { get; set; }
    }
}
