using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace absence_tracker.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation property to the Course class
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}