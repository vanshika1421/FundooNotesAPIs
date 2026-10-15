using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Notes.RequestModel
{
    public class NotesIdModel
    {
        [Required]
        public int NoteId { get; set; }


    }
}
