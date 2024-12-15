using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektPlaner.Models
{
    public class CalendarElement
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.", MinimumLength = 2)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        [Display(Name = "Miejsce")]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string? Location { get; set; }

        public string? Recurrence { get; set; } 

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        [Display(Name = "Grupa")]
        public string GroupId { get; set; }

        public CalendarGroup Group { get; set; }

    }
}
