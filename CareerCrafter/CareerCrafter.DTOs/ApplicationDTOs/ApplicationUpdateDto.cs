using System.ComponentModel.DataAnnotations;

namespace CareerCrafter.DTOs.ApplicationDTOs
{
    public class ApplicationUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}
