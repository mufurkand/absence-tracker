using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace absence_tracker.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public int TotalAbsences { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation property to the User class
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Navigation property to the Absence class
        public ICollection<Absence> Absences { get; set; } = new List<Absence>();
    }
}