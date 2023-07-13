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

        // GET: api/UserIdeaInteractions/tobi/0
        [HttpGet("{username}/{ideaId}")]
        public async Task<ActionResult<UserIdeaInteraction>> GetUserIdeaInteraction(string username, int ideaId)
        {
            if (_context.UserIdeaInteraction == null)
            {
                return NotFound();
            }
            var userIdeaInteraction = await _context.UserIdeaInteraction.FindAsync(username, ideaId);

            if (userIdeaInteraction == null)
            {
                return NotFound();
            }

            return userIdeaInteraction;
        }

        // PUT: api/UserIdeaInteractions/tobi/0
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{username}/{ideaId}")]
        public async Task<IActionResult> PutUserIdeaInteraction(
            string username,
            int ideaId,
            UserIdeaInteraction uii)
        {
            if (username != uii.Username || ideaId != uii.IdeaId)
            {
                return BadRequest();
            }

            _context.Entry(uii).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserIdeaInteractionExists(username,ideaId))
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
        public async Task<ActionResult<UserIdeaInteraction>> PostUserIdeaInteraction(UserIdeaInteraction uii)
        {
            if (_context.UserIdeaInteraction == null)
            {
                return Problem("Entity set 'seedsApiContext.UserIdeaInteraction'  is null.");
            }
            _context.UserIdeaInteraction.Add(uii);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserIdeaInteractionExists(uii.Username, uii.IdeaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserIdeaInteraction", new { id = uii.Username }, uii);
        }

        private bool UserIdeaInteractionExists(string username, int ideaId)
        {
            return (_context.UserIdeaInteraction?.Any(e => e.Username == username && e.IdeaId == ideaId))
                .GetValueOrDefault();
        }
    }
}
