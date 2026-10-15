using RepositoryLayer.Interface.Notes;
using System;
using System.Threading.Tasks;
using RepositoryLayer.Entity;
using RepositoryLayer.Context;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Notes.RequestModel;
namespace RepositoryLayer.Service.Notes
{
    public class NotesRL : INotesRL
    {
        private readonly FundooContext context;

        public NotesRL(FundooContext context)
        {
            this.context = context
                ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<NotesEntity> AddNotesRL(NotesCreationModel notes, int userId)
        {
            var entity = new NotesEntity
            {
                Title = notes.Title,
                Description = notes.Description,
                Color = notes.Color,
                UserId = userId,
                IsArchived = false,
                IsDeleted = false
            };
            await context.Notes.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
      
        public async Task<NotesEntity> ViewNotesByIdRL(int noteId, int userId)
        {
            var note = await context.Notes
                .Where(n => n.NoteId == noteId && n.UserId == userId && n.IsDeleted == false)
                .Select(n => new NotesEntity
                {
                    Title = n.Title,
                    Description = n.Description,
                    Color = n.Color,
                    
                })
                .FirstOrDefaultAsync();

            return note;
        }
        public async Task<List<NotesEntity>> ViewNotesRL(int userId)
        {
            var note = await context.Notes
                .Where(n => n.UserId == userId && n.IsDeleted == false)
                .Select(n => new NotesEntity
                {
                    NoteId = n.NoteId,
                    UserId = n.UserId,
                    Title = n.Title,
                    Description = n.Description,
                    Color = n.Color,
                    


                }).ToListAsync();
               
            return note;
        }
        public async Task<NotesEntity> DeleteNotesRL(int noteId, int userId)
        {
            var entity = await context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    !n.IsDeleted &&
                    n.UserId == userId);

            if (entity == null)
                return null;

            entity.IsDeleted = true;

            await context.SaveChangesAsync();

            return entity;
        }
        public async Task<NotesEntity> EditNotesRL(EditNotesModel notes, int userId)
        {
            var existingNote = await context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == notes.NoteId &&
                    n.UserId == userId &&
                    !n.IsDeleted);

            if (existingNote == null)
                return null;

            if (notes.Title != null)
                existingNote.Title = notes.Title;

            if (notes.Description != null)
                existingNote.Description = notes.Description;

            if (notes.Color != null)
                existingNote.Color = notes.Color;

            await context.SaveChangesAsync();

            return existingNote;
        }
        public async Task<NotesEntity> ArchiveNotesRL(int noteId, int userId)
        {
            var existingNote = await context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsDeleted);

            if (existingNote == null)
                return null;

            existingNote.IsArchived = !existingNote.IsArchived;

            await context.SaveChangesAsync();

            return existingNote;
        }
    }
}