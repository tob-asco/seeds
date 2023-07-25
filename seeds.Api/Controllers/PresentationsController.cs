using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentationsController : ControllerBase
    {
        private readonly seedsApiContext _context;

        public PresentationsController(seedsApiContext context)
        {
            _context = context;
        }

        // GET: api/Presentations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Presentation>> GetPresentationByIdeaId(int ideaId)
        {
            if (_context.Presentation == null)
            {
                return NotFound();
            }
            var presentation = await _context.Presentation.FirstOrDefaultAsync(
                p => p.IdeaId == ideaId);

            if (presentation == null)
            {
                return NotFound();
            }

            return presentation;
        }

        // PUT: api/Presentations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPresentation(int id, Presentation presentation)
        {
            if (id != presentation.Id)
            {
                return BadRequest();
            }

            _context.Entry(presentation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresentationExists(id))
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

        // POST: api/Presentations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Presentation>> PostPresentation(Presentation presentation)
        {
            if (_context.Presentation == null)
            {
                return Problem("Entity set 'seedsApiContext.Presentation'  is null.");
            }
            if (!IdeaExists(presentation.IdeaId))
            {
                return Problem($"Foreign key {nameof(Presentation.IdeaId)} not existing.");
            }
            _context.Presentation.Add(presentation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPresentationByIdeaId), new { ideaId = presentation.IdeaId }, presentation);
        }
        private bool PresentationExists(int id)
        {
            return (_context.Presentation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool IdeaExists(int id)
        {
            return (_context.Idea?.Any(i => i.Id == id)).GetValueOrDefault();
        }
    }
}
