using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektPlaner.Models
{
    public class CalendarGroup
    {

        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Display(Name = "Creator")]

        public string? FounderId { get; set; }

        public IdentityUser? Founder { get; set; }

        [Display(Name = "Members")]
        public IList<IdentityUser> Users { get; set; } = new List<IdentityUser>();

        [Display(Name = "Admins")]
        public IList<IdentityUser> Administrators { get; set; } = new List<IdentityUser>();

    }
}
