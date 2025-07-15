using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCrafter.DTOs.JobListingDTOs
{
    public class JobListingCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [StringLength(500)]
        public string Qualifications { get; set; }

        [Required]
        public decimal Salary { get; set; }

    }
}
