using ModelLayer.Notes.RequestModel;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using ModelLayer.Notes.ResponseModel;
namespace BusinessLayer.Interface.Notes
{
    public interface INotesBL
    {
        Task<NotesAddedResponseModel> AddNotesBL(NotesCreationModel notesEntity , int userId);
        Task<NotesEntity> ViewNotesByIdBL(int noteId, int userId);
        Task<List<NotesEntity>> ViewNotesBL(int userId);

        Task<NotesDeletedResponseModel> DeleteNotesBL(int noteId , int userId);
        Task<NotesEntity> EditNotesBL(EditNotesModel notes, int userId);
        Task<NotesArchieveResponseModel> ArchiveNotesBL(int noteId, int userId);
    }
}
