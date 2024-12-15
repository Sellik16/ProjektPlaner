using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektPlaner.Models
{
    public class CalendarElement
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string? Description { get; set; }
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Location")]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string? Location { get; set; }

        public string? UserId { get; set; }
            
        public IdentityUser? User { get; set; }

        [Display(Name = "Group")]
        public int? GroupId { get; set; } = null!;
        public CalendarGroup? Group { get; set; }

    }
}
