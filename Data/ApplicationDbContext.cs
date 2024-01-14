using BugTrackerMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BugTrackerMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "100", Name = "Administrator", NormalizedName = "ADMINISTRATOR" });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "500", Name = "User", NormalizedName = "USER" });
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = "1", NormalizedUserName = "admin@admin.admin", UserName = "admin@admin.admin", Email = "admin@admin.admin", EmailConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEFWp+7iGvNyb9NFG4gWnsdxmF+gfYlnDyGscUYyLSsG9Nr0f5F+Km7R4GAZzFq3IFA==" });
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = "2", NormalizedUserName = "test@test.test", UserName = "test@test.test", Email = "test@test.test", EmailConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEFWp+7iGvNyb9NFG4gWnsdxmF+gfYlnDyGscUYyLSsG9Nr0f5F+Km7R4GAZzFq3IFA==" });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "100", UserId = "1" });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "500", UserId = "2" });

            builder.Entity<ApplicationUser>()
           .HasMany(c => c.Bugs)
           .WithOne(e => e.ApplicationUser)
           .HasForeignKey(c => c.ApplicationUserId)
           .HasPrincipalKey(e => e.Id)
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Bug>()
           .HasMany(c => c.Comments)
           .WithOne(e => e.Bug)
           .HasForeignKey(c => c.BugId)
           .HasPrincipalKey(e => e.Id)
           .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
           .HasMany(c => c.SubComments)
           .WithOne(e => e.Comment)
           .HasForeignKey(c => c.CommentId)
           .HasPrincipalKey(e => e.Id)
           .OnDelete(DeleteBehavior.Cascade);

        }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }


    }
}
