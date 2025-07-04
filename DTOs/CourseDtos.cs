using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace absence_tracker.DTOs;

public class CourseDto
{
    /// <summary>
    /// Unique identifier for the course
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the course
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the course
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Total number of absences recorded for the course
    /// </summary>
    public int TotalAbsences { get; set; }

    /// <summary>
    /// Date and time when the course was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the course was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

public class CreateCourseDto
{
    /// <summary>
    /// Name of the course
    /// </summary>
    [Required]
    [Description("Name of the course")]
    [DefaultValue("Introduction to Programming")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the course
    /// </summary>
    [Required]
    [Description("Description of the course")]
    [DefaultValue("This course covers the basics of programming using C#.")]
    public string? Description { get; set; }
}

public class UpdateCourseDto : CreateCourseDto
{
}