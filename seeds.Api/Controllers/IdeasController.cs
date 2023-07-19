using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Model;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdeasController : ControllerBase
{
    private readonly seedsApiContext _context;

    public IdeasController(seedsApiContext context)
    {
        _context = context;
    }

    // GET: api/Ideas/page/5/size/20
    [HttpGet("page/{page}/size/{maxPageSize}")]
    public async Task<ActionResult<IEnumerable<Idea>>> GetIdeasPaginatedAsync(int page, int maxPageSize)
    {
        try
        {
            var totCount = await _context.Idea.CountAsync();

            if (totCount >= page * maxPageSize)
            { //at least one more maxPageSize of Ideas found
                var ideaPage = await _context.Idea
                    .Skip((page - 1) * maxPageSize)
                    .Take(maxPageSize)
                    .ToListAsync();
                return ideaPage != null ? ideaPage : NotFound();
            }
            else if (totCount > (page - 1) * maxPageSize)
            { //at least one more Idea found
              //return all ideas that are left
                var ideaPage = await _context.Idea
                    .Skip((page - 1) * maxPageSize)
                    .Take(totCount - ((page - 1) * maxPageSize))
                    .ToListAsync();
                return ideaPage != null ? ideaPage : NotFound();
            }
            else
            { //no more Ideas 
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/Ideas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Idea>> GetIdea(int id)
    {
        if (_context.Idea == null)
        {
            return NotFound();
        }
        var idea = await _context.Idea.FindAsync(id);

        if (idea == null)
        {
            return NotFound();
        }

        return idea;
    }

    // PUT: api/Ideas/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutIdeaAsync(int id, Idea idea)
    {
        if (id != idea.Id)
        {
            return BadRequest();
        }

        _context.Entry(idea).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!IdeaExists(id))
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

    // POST: api/Ideas
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Idea>> PostIdeaAsync(Idea idea)
    {
        if (_context.Idea == null)
        {
            return Problem("Entity set 'seedsApiContext.Idea'  is null.");
        }
        _context.Idea.Add(idea);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetIdea", new { id = idea.Id }, idea);
    }

    // DELETE: api/Ideas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIdea(int id)
    {
        if (_context.Idea == null)
        {
            return NotFound();
        }
        var idea = await _context.Idea.FindAsync(id);
        if (idea == null)
        {
            return NotFound();
        }

        _context.Idea.Remove(idea);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IdeaExists(int id)
    {
        return (_context.Idea?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
