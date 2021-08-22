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
    public class GroupsController : ControllerBase
    {
        private readonly NoteContext _context;

        public GroupsController(NoteContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups(int userId)
        {
            var user = _context.Users.SingleOrDefault(user => user.UserId == userId);

            return await _context.Groups
                .Include(group => group.Users)
                .Where(group => group.Users.Contains(user))
                .ToListAsync();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Group>> GetGroup(int id, int userId)
        {
            var @group = await _context.Groups
                .Where(group => group.CreatorId == userId)
                .SingleOrDefaultAsync(group => group.GroupId == id);

            if (@group == null)
            {
                return NotFound();
            }

            return @group;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutGroup(int id, Group @group)
        {
            if (id != @group.GroupId)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGroup", new { id = @group.GroupId }, @group);
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            _context.Groups.Add(@group);
            var creator = _context.Users.SingleOrDefault(user => user.UserId == @group.CreatorId);
            creator.Groups.Add(@group);
            @group.Users.Add(creator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroup", new { id = @group.GroupId }, @group);
        }

        [HttpPost("join")]
        [Authorize]
        public async Task<ActionResult<Group>> JoinGroup(JoinGroup @joinGroup)
        {
            var user = _context.Users.Single(user => user.UserId == @joinGroup.UserId);
            var group = _context.Groups.Single(group => group.GroupId == @joinGroup.GroupId);
            group.Users.Add(user);
            group.CountOfUsers++;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("leave")]
        [Authorize]
        public async Task<ActionResult<Group>> LeaveGroup(JoinGroup @joinGroup)
        {
            var user = _context.Users
                .Include(user => user.Groups)
                .SingleOrDefault(user => user.UserId == @joinGroup.UserId);

            if (user != null)
            {
                foreach (var group in user.Groups
                    .Where(g => g.GroupId == joinGroup.GroupId).ToList())
                {
                    user.Groups.Remove(group);
                    group.Users.Remove(user);
                    group.CountOfUsers--;
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }

        private bool isUserInCollection(IEnumerable<User> users, int userId)
        {
            foreach (var user in users)
            {
                if (user.UserId == userId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
