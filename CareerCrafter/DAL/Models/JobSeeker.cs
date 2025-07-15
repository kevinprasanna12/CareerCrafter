using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class JobSeeker
    {
        [Key]
        public int JobSeekerId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        public Resume Resume { get; set; }

        public ICollection<Application> Applications { get; set; }

        [Required]
        public int UserId { get; set; }  

        [ForeignKey("UserId")]
        public UserInfo User { get; set; }
    }
}
