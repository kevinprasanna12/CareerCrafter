using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Resume
    {
        [Key]
        public int ResumeId { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }

        [Required]
        public int JobSeekerId { get; set; }

        [ForeignKey("JobSeekerId")]
        public JobSeeker JobSeeker { get; set; }
    }
}
