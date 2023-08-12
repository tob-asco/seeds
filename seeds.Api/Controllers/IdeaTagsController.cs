using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdeaTagsController : ControllerBase
{
    private readonly seedsApiContext _context;

    public IdeaTagsController(seedsApiContext context)
    {
        _context = context;
    }

    // GET: api/IdeaTags/0/NoC/tag
    /*
    [HttpGet("{ideaId}/{catKey}/{tagName}")]
    public async Task<ActionResult<IdeaTag>> GetIdeaTag(
        int ideaId, string catKey, string tagName)
    {
        if (_context.IdeaTag == null) { return NotFound(); }

        var ideaTag = await _context.IdeaTag.FindAsync(
            ideaId, catKey, tagName);

        if (ideaTag == null) { return NotFound(); }

        return ideaTag;
    }
    */

    // GET: api/IdeaTags/0

    [HttpGet("{ideaId}")]
    public async Task<ActionResult<List<IdeaTag>>> GetTagsOfIdea(int ideaId)
    {
        if (_context.IdeaTag == null) { return NotFound(); }

        var ideaTags = _context.IdeaTag.Where(it => it.IdeaId == ideaId);

        if (ideaTags == null)
        {
            return new List<IdeaTag>();
        }

        return await ideaTags.ToListAsync();
    }

    // PUT: api/IdeaTags/5
    /*
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{ideaId}/{catKey}/{tagName}")]
    public async Task<IActionResult> PutIdeaTag(
        int ideaId, string catKey, string tagName, IdeaTag ideaTag)
    {
        if (ideaId != ideaTag.IdeaId ||
            catKey != ideaTag.CategoryKey ||
            tagName != ideaTag.TagName) { return BadRequest(); }

        _context.Entry(ideaTag).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!IdeaTagExists(ideaId, catKey, tagName))
            {
                return NotFound();
            }
            else { throw; }
        }

        return NoContent();
    }
    */

    // POST: api/IdeaTags
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<IdeaTag>> PostIdeaTag(IdeaTag ideaTag)
    {
        if (_context.IdeaTag == null)
        {
            return Problem("Entity set 'seedsApiContext.IdeaTag'  is null.");
        }
        _context.IdeaTag.Add(ideaTag);
        try
        {
            if (IdeaTagExists(ideaTag.IdeaId, ideaTag.TagId))
            {
                return Conflict();
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

        return Ok(); //No CreatedAtAction because not yet needed
    }

    // DELETE: api/IdeaTags/0/NoC/tag
    [HttpDelete("{ideaId}/{catKey}/{tagName}")]
    public async Task<IActionResult> DeleteIdeaTag(int ideaId, string catKey, string tagName)
    {
        if (_context.IdeaTag == null) { return NotFound(); }

        catKey = HttpUtility.UrlDecode(catKey);
        tagName = HttpUtility.UrlDecode(tagName);
        var ideaTag = await _context.IdeaTag.FindAsync(ideaId, catKey, tagName);
        if (ideaTag == null) { return NotFound(); }

        _context.IdeaTag.Remove(ideaTag);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IdeaTagExists(int ideaId, Guid tagId)
    {
        return (_context.IdeaTag?.Any(e =>
            e.IdeaId == ideaId && e.TagId == tagId
            )).GetValueOrDefault();
    }
}
