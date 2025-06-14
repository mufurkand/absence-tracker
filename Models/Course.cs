using System.ComponentModel.DataAnnotations;

namespace absence_tracker.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }
        public int TotalAbsences { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property to the User class
        public User? User { get; set; }
        // Navigation property to the Absence class
        public ICollection<Absence> Absences { get; set; } = new List<Absence>();
    }
}