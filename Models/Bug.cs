using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerMVC.Models
{
    public class Bug
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Autor ID' jest wymagane.")]
        public string AuthorId { get; set; }

        [Required(ErrorMessage = "Pole 'Tytuł' jest wymagane.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Pole 'Status' jest wymagane.")]
        public string Status { get; set; } = "Open";

        [Required(ErrorMessage = "Pole 'Opis' jest wymagane.")]
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
