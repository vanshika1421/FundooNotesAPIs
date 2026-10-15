using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.Notes.ResponseModel
{
    public class NotesArchieveResponseModel
    {
        public bool Success{ get; set; } = true;
        public string Message { get; set; } = "Notes Archieved Successfully";
    }
}
