using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int JobSeekerId { get; set; }

        [ForeignKey("JobSeekerId")]
        public JobSeeker JobSeeker { get; set; }

        [Required]
        public int JobListingId { get; set; }

        [ForeignKey("JobListingId")]
        public JobListing JobListing { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}
