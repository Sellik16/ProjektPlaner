using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektPlaner.Models;

namespace ProjektPlaner.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CalendarElement> CalendarElement { get; set; } = default!;
        public DbSet<CalendarGroup> CalendarGroup { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CalendarGroup>()
                .HasMany(g => g.Users)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "CalendarGroupUser",
                    j => j.HasOne<IdentityUser>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<CalendarGroup>().WithMany().HasForeignKey("CalendarGroupId"));

            builder.Entity<CalendarGroup>()
                .HasMany(g => g.Administrators)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "CalendarGroupAdministrator",
                    j => j.HasOne<IdentityUser>().WithMany().HasForeignKey("AdminId"),
                    j => j.HasOne<CalendarGroup>().WithMany().HasForeignKey("CalendarGroupId"));

            builder.Entity<CalendarElement>()
                .HasOne(e => e.Group)
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        
    }
    }
}