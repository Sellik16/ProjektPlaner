using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjektPlaner.Models
{
    public class CalendarElement
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [Length(1, 255, ErrorMessage = "Chyba cie pojebalo co tak dlugo")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [Length(1, 512, ErrorMessage = "Chyba cie pojebalo co tak dlugo")]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        [Display(Name = "Miejsce")]
        [Range(1, 255, ErrorMessage = "Chyba cie pojebalo co tak dlugo")]
        public string? Location { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }



    }
}
