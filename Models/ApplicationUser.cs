namespace BugTrackerMVC.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ICollection<Bug> Bugs { get; set; } = new List<Bug>();
    }

}
