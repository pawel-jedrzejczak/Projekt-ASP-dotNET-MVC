using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerMVC.Models
{
    public class SubComment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole 'Autor ID' jest wymagane.")]
        public string AuthorId { get; set; }
        [Required(ErrorMessage = "Pole 'Tytuł' jest wymagane.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Pole 'Zawartość' jest wymagane.")]
        public string Content { get; set; }

        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }


    }
}
