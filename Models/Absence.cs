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
        public DateOnly Date { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation property to the Course class
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}