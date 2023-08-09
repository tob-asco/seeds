using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;

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

    // PUT: api/IdeaTags/5
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
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateException)
        {
            if (IdeaTagExists(ideaTag.IdeaId, ideaTag.CategoryKey, ideaTag.TagName))
            {
                return Conflict();
            }
            else { throw; }
        }

        return CreatedAtAction(
            "GetIdeaTag",
            new { ideaTag.IdeaId, ideaTag.CategoryKey, ideaTag.TagName },
            ideaTag);
    }

    // DELETE: api/IdeaTags/5
    [HttpDelete("{ideaId}/{catKey}/{tagName}")]
    public async Task<IActionResult> DeleteIdeaTag(
        int ideaId, string catKey, string tagName)
    {
        if (_context.IdeaTag == null) { return NotFound(); }
        var ideaTag = await _context.IdeaTag.FindAsync(ideaId, catKey, tagName);
        if (ideaTag == null) { return NotFound(); } 

        _context.IdeaTag.Remove(ideaTag);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IdeaTagExists(int ideaId, string catKey, string tagName)
    {
        return (_context.IdeaTag?.Any(e =>
            e.IdeaId == ideaId && e.CategoryKey == catKey && e.TagName == tagName
            )).GetValueOrDefault();
    }
}
