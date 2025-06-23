using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace absence_tracker.DTOs
{
    public class AbsenceDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // CourseId is included in the DTO and not fetched from URL parameters because absences have a
    // seperate controller.
    public class CreateAbsenceDto
    {
        [Required]
        [Description("Id of the course")]
        public int CourseId { get; set; }

        // TODO: use DateOnly?
        [Required]
        [Description("Absence date (will be converted to UTC if not already)")]
        [DefaultValue(typeof(DateOnly), "2025-06-23")]
        public DateOnly Date { get; set; }

        [MaxLength(500)]
        [Description("Reason of the absence")]
        [DefaultValue("Fell asleep")]
        public string? Reason { get; set; }
    }
}