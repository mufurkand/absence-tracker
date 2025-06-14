using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace absence_tracker.Models
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        // Navigation property to the Course class
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}