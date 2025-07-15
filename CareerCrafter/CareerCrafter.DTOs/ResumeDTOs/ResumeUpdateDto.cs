using System.ComponentModel.DataAnnotations;

namespace CareerCrafter.DTOs.ResumeDTOs
{
    public class ResumeUpdateDto
    {
        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }

        [Required]
        public int JobSeekerId { get; set; }
    }
}
