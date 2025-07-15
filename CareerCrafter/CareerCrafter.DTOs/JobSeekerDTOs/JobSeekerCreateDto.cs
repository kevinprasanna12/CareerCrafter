using System.ComponentModel.DataAnnotations;

namespace CareerCrafter.DTOs.JobSeekerDTOs
{
    public class JobSeekerCreateDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
