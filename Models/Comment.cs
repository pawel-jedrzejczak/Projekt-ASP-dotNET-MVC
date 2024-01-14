using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Autor ID' jest wymagane.")]
        public string AuthorId { get; set; }
        [Required(ErrorMessage = "Pole 'Tytuł' jest wymagane.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Pole 'Zawartość' jest wymagane.")]
        public string Content { get; set; }

        public ICollection<SubComment> SubComments { get; set; } = new List<SubComment>();

        [ForeignKey("Bug")]
        public int BugId { get; set; }
        public Bug? Bug { get; set; }

 
    }

}
