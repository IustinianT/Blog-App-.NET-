using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations;

namespace Blog_App.Models
{
    public class Note
    {

        public int Id { get; set; }
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }

        [Required]
        public string NoteAuthor { get; set; }

        public Note() { }
    }
}
