using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Model;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdeasController : ControllerBase
{
    private readonly seedsApiContext _context;
    private readonly IMapper _mapper;

    public IdeasController(
        seedsApiContext context,
        IMapper mapper)
    {
        _context = context;
        this._mapper = mapper;
    }

    // GET: api/Ideas/page/5/size/20
    [HttpGet("page/{page}/size/{maxPageSize}")]
    public async Task<ActionResult<IEnumerable<IdeaFromDb>>> GetIdeasPaginated(
        int page, int maxPageSize)
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
                var ideaDtoPage = _mapper.Map<List<IdeaFromDb>>(ideaPage);
                return ideaDtoPage != null ? ideaDtoPage : NotFound();
            }
            else if (totCount > (page - 1) * maxPageSize)
            { //at least one more Idea found
              //return all ideas that are left
                var ideaPage = await _context.Idea
                    .Skip((page - 1) * maxPageSize)
                    .Take(totCount - ((page - 1) * maxPageSize))
                    .ToListAsync();
                var ideaDtoPage = _mapper.Map<List<IdeaFromDb>>(ideaPage);
                return ideaDtoPage != null ? ideaDtoPage : NotFound();
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
    public async Task<ActionResult<IdeaFromDb>> GetIdea(int id)
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

        var ideaDto = _mapper.Map<IdeaFromDb>(idea);

        return ideaDto;
    }

    // PUT: api/Ideas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutIdea(int id, IdeaFromDb ideaDto)
    {
        if (id != ideaDto.Id)
        {
            return BadRequest();
        }

        var idea = _mapper.Map<Idea>(ideaDto);

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
    [HttpPost]
    public async Task<ActionResult<Idea>> PostIdea(IdeaToDb ideaDto)
    {
        if (_context.Idea == null)
        {
            return Problem("Entity set 'seedsApiContext.Idea'  is null.");
        }

        Idea idea = _mapper.Map<Idea>(ideaDto);

        _context.Idea.Add(idea); // this updates idea! (test this; yess)
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
