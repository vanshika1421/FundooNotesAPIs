namespace ModelLayer.Notes.ResponseModel
{
    public class NotesAddedResponseModel
    {
        public bool Success { get; set; } = true;

        public string Message { get; set; }    = "Note added successfully.";
    }
}