using System.ComponentModel.DataAnnotations;

namespace absence_tracker.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Navigation property to the Course class
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}