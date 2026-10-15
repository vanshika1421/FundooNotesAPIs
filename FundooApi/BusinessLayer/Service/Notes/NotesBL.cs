using BusinessLayer.Interface.Notes;
using RepositoryLayer.Interface.Notes; // <-- replace with the actual namespace of INotesRL
using RepositoryLayer.Entity;
using System.Threading.Tasks;
using ModelLayer.Notes.ResponseModel;
using ModelLayer.Notes.RequestModel;    

namespace BusinessLayer.Service.Notes
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL notesRL;

        public NotesBL(INotesRL notesRL)
        {
            this.notesRL = notesRL;
        }
        public async Task<NotesDeletedResponseModel> DeleteNotesBL(int NotesId, int userId)
        {
            NotesEntity deleted = await notesRL.DeleteNotesRL(NotesId, userId);
            if (deleted == null)
            {
                return new NotesDeletedResponseModel
                {
                    Success = false,
                    Message = "Failed to delete note."
                };
            }

            return new NotesDeletedResponseModel
            {
                Success = true,
                Message = "Note deleted successfully."
            };
            
        }

        public async Task<NotesAddedResponseModel> AddNotesBL(NotesCreationModel notesEntity , int userId)
        {
            NotesEntity res =  await notesRL.AddNotesRL(notesEntity, userId);
            if (res == null)
            {
                return new NotesAddedResponseModel
                {
                    Success = false,
                    Message = "Failed to add note."
                };
            }

            return new NotesAddedResponseModel
            {
                Success = true,
                Message = "Note added successfully."
            };

        }
        public async Task<NotesEntity> ViewNotesByIdBL(int notesId, int userId)
        {
            return await notesRL.ViewNotesByIdRL(notesId, userId);
        }
        public async Task<List<NotesEntity>> ViewNotesBL(int userId)
        {
            return await notesRL.ViewNotesRL(userId);
        }
        public async Task<NotesEntity> EditNotesBL(EditNotesModel notes, int userId)
        {
            return await notesRL.EditNotesRL(notes, userId);
        }
        public async Task<NotesArchieveResponseModel> ArchiveNotesBL(int noteId, int userId)
        {
            NotesEntity archieveEntity = await notesRL.ArchiveNotesRL(noteId, userId);
            if (archieveEntity == null)
            {
                return new NotesArchieveResponseModel
                {
                    Success = false,
                    Message = "Failed to archive note."
                };
            }
            return new NotesArchieveResponseModel
            {
                Success = true,
                Message = "Note archived successfully."
            };
        }
        
    }
}
