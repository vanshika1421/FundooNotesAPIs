using ModelLayer.Notes.RequestModel;
using RepositoryLayer.Entity;
using RepositoryLayer.Service.Notes;
using System;
using System.Collections.Generic;
using System.Text;
namespace RepositoryLayer.Interface.Notes
{
    public interface INotesRL
    {
        Task<NotesEntity> AddNotesRL(NotesCreationModel notes, int userId);

         Task<NotesEntity> ViewNotesByIdRL(int noteId, int userId);

         Task<List<NotesEntity>> ViewNotesRL(int userId);
        Task<NotesEntity> DeleteNotesRL(int noteId , int userId);
        Task<NotesEntity> EditNotesRL(EditNotesModel notes, int userId);
        Task<NotesEntity> ArchiveNotesRL(int noteId, int userId);
    }
}
