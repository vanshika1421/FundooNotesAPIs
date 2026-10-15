using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.Notes.RequestModel
{
    public class EditNotesModel
    {
        [Required]
        public int NoteId { get; set; } 

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Color { get; set; }  = string.Empty;
    }
}
