using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace Blog_App.Models
{
    public class Note
    {

        public int Id { get; set; }
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }

        public Note() { }
    }
}
