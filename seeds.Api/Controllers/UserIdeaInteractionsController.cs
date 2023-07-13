using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserIdeaInteractionsController : ControllerBase
    {
        private readonly seedsApiContext _context;

        public UserIdeaInteractionsController(seedsApiContext context)
        {
            _context = context;
        }

        // GET: api/UserIdeaInteractions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserIdeaInteraction>>> GetUserIdeaInteraction()
        {
          if (_context.UserIdeaInteraction == null)
          {
              return NotFound();
          }
            return await _context.UserIdeaInteraction.ToListAsync();
        }

        // GET: api/UserIdeaInteractions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserIdeaInteraction>> GetUserIdeaInteraction(string id)
        {
          if (_context.UserIdeaInteraction == null)
          {
              return NotFound();
          }
            var userIdeaInteraction = await _context.UserIdeaInteraction.FindAsync(id);

            if (userIdeaInteraction == null)
            {
                return NotFound();
            }

            return userIdeaInteraction;
        }

        // PUT: api/UserIdeaInteractions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserIdeaInteraction(string id, UserIdeaInteraction userIdeaInteraction)
        {
            if (id != userIdeaInteraction.Username)
            {
                return BadRequest();
            }

            _context.Entry(userIdeaInteraction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserIdeaInteractionExists(id))
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

        // POST: api/UserIdeaInteractions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserIdeaInteraction>> PostUserIdeaInteraction(UserIdeaInteraction userIdeaInteraction)
        {
          if (_context.UserIdeaInteraction == null)
          {
              return Problem("Entity set 'seedsApiContext.UserIdeaInteraction'  is null.");
          }
            _context.UserIdeaInteraction.Add(userIdeaInteraction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserIdeaInteractionExists(userIdeaInteraction.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserIdeaInteraction", new { id = userIdeaInteraction.Username }, userIdeaInteraction);
        }

        // DELETE: api/UserIdeaInteractions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserIdeaInteraction(string id)
        {
            if (_context.UserIdeaInteraction == null)
            {
                return NotFound();
            }
            var userIdeaInteraction = await _context.UserIdeaInteraction.FindAsync(id);
            if (userIdeaInteraction == null)
            {
                return NotFound();
            }

            _context.UserIdeaInteraction.Remove(userIdeaInteraction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserIdeaInteractionExists(string id)
        {
            return (_context.UserIdeaInteraction?.Any(e => e.Username == id)).GetValueOrDefault();
        }
    }
}
