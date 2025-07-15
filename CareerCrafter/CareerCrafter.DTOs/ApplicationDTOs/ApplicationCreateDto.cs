using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCrafter.DTOs.ApplicationDTOs
{
    public class ApplicationCreateDto
    {
        [Required]
        public int JobSeekerId { get; set; }

        [Required]
        public int JobListingId { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}
