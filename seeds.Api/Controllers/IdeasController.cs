using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Api.Pages;
using seeds.Dal.Dto.ForMaui;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Model;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdeasController : ControllerBase
{
    private readonly seedsApiContext context;
    private readonly IMapper mapper;

    public IdeasController(
        seedsApiContext context,
        IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    // GET: api/Ideas/page/5?isDescending=false&pageSize=20
    [HttpGet("page/{pageIndex}")]
    public async Task<ActionResult<IEnumerable<IdeaFromDb>>> GetIdeasPaginated(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        try
        {
            var validColumns = new[] {
                nameof(IdeaFromDb.Id).ToLower(),
                nameof(IdeaFromDb.CreationTime).ToLower()
            };
            if (!validColumns.Contains(orderByColumn.ToLower()))
            {
                return BadRequest("Invalid column name for sorting.");
            }

            IQueryable<Idea> query = context.Idea;

            // Apply dynamic sorting
            var orderDirection = isDescending ? "descending" : "ascending";
            query = query.OrderBy($"{orderByColumn} {orderDirection}");

            // Apply pagination. Take(pageSize) should take #(<= pageSize) ideas
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            // using Linq.Dynamic.Core's query will only lazily access the DB,
            // once the query is enumerated, which happens now:
            var ideas = mapper.Map<List<IdeaFromDb>>(await query.ToListAsync());

            return Ok(ideas);
        }
        catch (Exception ex)
        {
            // Handle exceptions appropriately
            return StatusCode(500, ex.Message);
        }
    }

    // GET: api/Ideas/feedentryPage/5?isDescending=false&pageSize=20
    [HttpGet("feedentryPage/{pageIndex}")]
    public async Task<ActionResult<List<Feedentry>>> GetFeedentriesPaginated(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        try
        {
            var validColumns = new[] {
                nameof(IdeaFromDb.Id).ToLower(),
                nameof(IdeaFromDb.CreationTime).ToLower()
            };
            if (!validColumns.Contains(orderByColumn.ToLower()))
            {
                return BadRequest("Invalid column name for sorting.");
            }

            /* The following looks cumbersome, why not simply use "i => i"?
             * Problem is that we want to directly include i.Tags, which the following does.
             * However, we can't simply ".Include(i => i.Tags)" (ad infinitum inclusion).
             * So here, we project to the first layer of Nav. Props. (GPT recommended)
             * Pro: We won't include stuff that we don't need.
             */
            IQueryable<Idea> query = context.Idea
                .Select(i => new Idea
                {
                    Id = i.Id,
                    Title = i.Title,
                    Slogan = i.Slogan,
                    CreatorName = i.CreatorName,
                    CreationTime = i.CreationTime,
                    Slide1 = i.Slide1,
                    Slide2 = i.Slide2,
                    Slide3 = i.Slide3,
                    // Include other properties you need from Idea
                    Tags = i.Tags // This will load the associated tags for each idea
                });

            // Apply dynamic sorting
            var orderDirection = isDescending ? "descending" : "ascending";
            query = query.OrderBy($"{orderByColumn} {orderDirection}");

            // Apply pagination. Take(pageSize) should take #(<= pageSize) ideas
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            if (!await query.AnyAsync()) { return new List<Feedentry>(); }
            // using Linq.Dynamic.Core's query will only lazily access the DB,
            // once the query is enumerated, which happens now:
            var ideas = await query.ToListAsync();

            List<Feedentry> fes = new();
            foreach (var idea in ideas)
            {
                var upvoteCountForIdea = context.UserIdeaInteraction
                    .GroupBy(uii => uii.IdeaId)
                    .Select(group => new
                    {
                        IdeaId = group.Key,
                        UpvoteCount = group.Sum(
                            idea => (idea.Upvoted ? 1 : 0) + (idea.Downvoted ? -1 : 0))
                    })
                    .FirstOrDefault(uii => uii.IdeaId == idea.Id);
                fes.Add(new()
                {
                    Idea = mapper.Map<IdeaFromDb>(idea),
                    Tags = mapper.Map<List<TagFromDb>>(idea.Tags),
                    Upvotes = upvoteCountForIdea != null ? upvoteCountForIdea.UpvoteCount : 0
                });
            }
            return fes;
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET: api/Ideas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<IdeaFromDb>> GetIdea(int id)
    {
        if (context.Idea == null)
        {
            return NotFound();
        }
        var idea = await context.Idea.FindAsync(id);

        if (idea == null)
        {
            return NotFound();
        }

        var ideaDto = mapper.Map<IdeaFromDb>(idea);

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

        var idea = mapper.Map<Idea>(ideaDto);

        context.Entry(idea).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
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
        if (context.Idea == null)
        {
            return Problem("Entity set 'seedsApiContext.Idea'  is null.");
        }

        Idea idea = mapper.Map<Idea>(ideaDto);

        context.Idea.Add(idea); // this updates idea! (test this; yess)
        await context.SaveChangesAsync();

        return CreatedAtAction("GetIdea", new { id = idea.Id }, idea);
    }

    // DELETE: api/Ideas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIdea(int id)
    {
        if (context.Idea == null)
        {
            return NotFound();
        }
        var idea = await context.Idea.FindAsync(id);
        if (idea == null)
        {
            return NotFound();
        }

        context.Idea.Remove(idea);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool IdeaExists(int id)
    {
        return (context.Idea?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
