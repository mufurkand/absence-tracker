using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace absence_tracker.Models
{
    public class Absence
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to the Course class
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}