using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace CareerCrafterMVC.Models
{
    public class UserInfo
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

  
        [StringLength(20)]
        public string Role { get; set; } = "Employer";


        [ValidateNever]
        // Navigation Property
        public Employee Employee { get; set; }
    }
}
