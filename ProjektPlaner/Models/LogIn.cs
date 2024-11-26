using Microsoft.AspNetCore.Identity;

namespace ProjektPlaner.Models
{
    public class LogIn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IdentityUser? User { get; set; }
    }
}
