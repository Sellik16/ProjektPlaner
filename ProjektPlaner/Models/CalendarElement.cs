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

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; } // Data rozpoczęcia wydarzenia

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; } // Data zakończenia wydarzenia

        [Display(Name = "Location")]
        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
        public string? Location { get; set; }

        public string? UserId { get; set; } // Id użytkownika, który stworzył wydarzenie

        public IdentityUser? User { get; set; }

        [Display(Name = "Group")]
        public int? GroupId { get; set; } = null!;

        public CalendarGroup? Group { get; set; } // Powiązana grupa wydarzenia
    }
}
