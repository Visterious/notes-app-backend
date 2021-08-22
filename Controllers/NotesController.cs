using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;

namespace NotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext _context;

        public NotesController(NoteContext context)
        {
            _context = context;
        }

        // GET: api/Notes/5
        [HttpGet("{groupId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes(int groupId)
        {
            return await _context.Notes.Where(note => note.GroupId == groupId).ToListAsync();
        }

        // GET: api/Notes/5/5
        [HttpGet("{groupId}/{id}")]
        [Authorize]
        public async Task<ActionResult<Note>> GetNote(int groupId, int id)
        {
            var note = await _context.Notes
                .Where(note => note.GroupId == groupId)
                .SingleAsync(note => note.NoteId == id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // PUT: api/Notes/5/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{groupId}/{id}")]
        [Authorize]
        public async Task<IActionResult> PutNote(int groupId, int id, Note note)
        {
            if (id != note.NoteId)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNote", new { groupId = note.GroupId, id = @note.NoteId }, @note);
        }

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Note>> PostNote(Note @note)
        {
            if (!_context.Groups.Any(g => g.GroupId == @note.GroupId))
            {
                return NotFound(new { message = "Not group" });
            }

            var user = _context.Users.Single(user => user.UserId == @note.UserId);
            var group = _context.Groups
                .Include(group => group.Users).Single(group => group.GroupId == @note.GroupId);

            if (!group.Users.Any(u => u.UserId == @note.UserId))
            {
                return NotFound(new { message = "Not user" });
            }

            @note.Group = group;
            @note.User = user;
            group.Notes.Add(@note);
            _context.Notes.Add(@note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { groupId = note.GroupId, id = @note.NoteId }, @note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{groupId}/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNote(int groupId, int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.NoteId == id);
        }
    }
}
