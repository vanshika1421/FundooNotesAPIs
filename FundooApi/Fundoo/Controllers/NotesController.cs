using BusinessLayer.Interface.Notes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Notes.RequestModel;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Fundoo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    
    public class NotesController : ControllerBase
    {
        public const string Nameidentifier = "userId";
        public INotesBL _notesBL;
        public FundooContext context;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ILogger<NotesController> _logger;

        public NotesController(INotesBL notesBL, FundooContext context, ILogger<NotesController> logger)
        {
            _notesBL = notesBL;
            this.context = context;
            _logger = logger;
        }

        [HttpPost("add-notes")]
        public async Task<ActionResult> AddNote(NotesCreationModel notesEntity)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("Invalid or missing user ID claim.");
            }

            var result = await _notesBL.AddNotesBL(notesEntity , userId);
            if(result == null)
            {
                return BadRequest("Failed to add note.");
            }
            return Ok(result);
        }
        [HttpGet("view-notes-by-id")]
        public async Task<ActionResult> ViewNotesById(int noteId )
        {
            if(!TryGetUserId(out int userId))
            {
                return Unauthorized("Invalid or missing userIdClam.");
            }
            var result =  await _notesBL.ViewNotesByIdBL(noteId  , userId);
            return Ok(result);
        }

        [HttpGet("view-notes")]
        public async Task<ActionResult> ViewNotes()
        {
            if(!TryGetUserId(out int userId))
            {
                return Unauthorized("Invalid or missing userIdClam.");
            }
            var result = await _notesBL.ViewNotesBL(userId);
            return Ok(result);
        }

        [HttpDelete("delete-notes")]
        public async Task<ActionResult> DeleteNotes(int noteId)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized();
            }

            var deleted = await _notesBL.DeleteNotesBL(noteId, userId);
            if(deleted == null)
            {
                return NotFound("Note not found or you do not have permission to delete it.");
            }
            return Ok(deleted);
        }

        [HttpPut("edit-notes")]
        public async Task<ActionResult> editNotes([FromBody]EditNotesModel notes)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized();
            }

           return Ok(await _notesBL.EditNotesBL(notes, userId));
            
        }

        [HttpPut("archieve")]
        public async Task<ActionResult> ArchieveNotes(int noteId)
        {
            

            try
            {
                if (!TryGetUserId(out int userId))
                {

                    return Unauthorized();
                }
                return Ok(await _notesBL.ArchiveNotesBL(noteId, userId));

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while archiving NoteId: {NoteId}", noteId);

                return StatusCode(500,"An error occurred while archiving the note.");
            }
        }

        private bool TryGetUserId(out int userId)
        {


            return int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);
        }
    }
}