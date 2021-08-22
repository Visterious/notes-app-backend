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
    public class InvitesController : ControllerBase
    {
        private readonly NoteContext _context;

        public InvitesController(NoteContext context)
        {
            _context = context;
        }

        // GET: api/Invites
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Invite>>> GetInvite(int userId)
        {
            return await _context.Invite.Where(invite => invite.InvitedId == userId).ToListAsync();
        }

        // GET: api/Invites/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Invite>> GetInvite(int id, int userId)
        {
            var invite = await _context.Invite
                .Where(invite => invite.InvitedId == userId)
                .SingleOrDefaultAsync(invite => invite.InviteId == id);

            if (invite == null)
            {
                return NotFound();
            }

            return invite;
        }

        // PUT: api/Invites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutInvite(int id, Invite invite)
        {
            if (id != invite.InviteId)
            {
                return BadRequest();
            }

            _context.Entry(invite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InviteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Invites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Invite>> PostInvite(Invite invite)
        {
            _context.Invite.Add(invite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvite", new { id = invite.InviteId }, invite);
        }

        // DELETE: api/Invites/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteInvite(int id)
        {
            var invite = await _context.Invite.FindAsync(id);
            if (invite == null)
            {
                return NotFound();
            }

            _context.Invite.Remove(invite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InviteExists(int id)
        {
            return _context.Invite.Any(e => e.InviteId == id);
        }
    }
}
