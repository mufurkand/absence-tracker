using System.ComponentModel.DataAnnotations;

namespace absence_tracker.Models
{
    public class Absence
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property to the Course class
        public Course? Course { get; set; }
    }
}