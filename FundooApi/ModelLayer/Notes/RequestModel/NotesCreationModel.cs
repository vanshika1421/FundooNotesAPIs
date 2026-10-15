using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Notes.RequestModel
{
    public class NotesCreationModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;
    }
}