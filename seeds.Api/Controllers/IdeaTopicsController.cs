using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdeaTopicsController : ControllerBase
{
    private readonly seedsApiContext _context;
    private readonly IMapper mapper;

    public IdeaTopicsController(
        seedsApiContext context,
        IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }

    // GET: api/IdeaTopics/0/NoC/topic
    /*
    [HttpGet("{ideaId}/{catKey}/{topicName}")]
    public async Task<ActionResult<IdeaTopic>> GetIdeaTopic(
        int ideaId, string catKey, string topicName)
    {
        if (_context.IdeaTopic == null) { return NotFound(); }

        var ideaTopic = await _context.IdeaTopic.FindAsync(
            ideaId, catKey, topicName);

        if (ideaTopic == null) { return NotFound(); }

        return ideaTopic;
    }
    */

    // GET: api/IdeaTopics/0

    [HttpGet("{ideaId}")]
    public async Task<ActionResult<List<TopicFromDb>>> GetTopicsOfIdea(int ideaId)
    {
        if (_context.IdeaTopic == null) { return NotFound(); }

        var idea = await _context.Idea
            .Include(i => i.Topics) // this populates the Navigation property
            .FirstOrDefaultAsync(i => i.Id == ideaId);
        if (idea == null) { return NotFound(); }

        var topics = idea.Topics.ToList();
        var topicsDto = mapper.Map<List<TopicFromDb>>(topics);
        return topicsDto;
    }

    // PUT: api/IdeaTopics/5
    /*
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{ideaId}/{catKey}/{topicName}")]
    public async Task<IActionResult> PutIdeaTopic(
        int ideaId, string catKey, string topicName, IdeaTopic ideaTopic)
    {
        if (ideaId != ideaTopic.IdeaId ||
            catKey != ideaTopic.CategoryKey ||
            topicName != ideaTopic.TopicName) { return BadRequest(); }

        _context.Entry(ideaTopic).State = EntityState.Modified;

        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!IdeaTopicExists(ideaId, catKey, topicName))
            {
                return NotFound();
            }
            else { throw; }
        }

        return NoContent();
    }
    */

    // POST: api/IdeaTopics
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<IdeaTopic>> PostIdeaTopic(IdeaTopic ideaTopic)
    {
        if (_context.IdeaTopic == null)
        {
            return Problem("Entity set 'seedsApiContext.IdeaTopic'  is null.");
        }
        _context.IdeaTopic.Add(ideaTopic);
        try
        {
            if (IdeaTopicExists(ideaTopic.IdeaId, ideaTopic.TopicId))
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

    // DELETE: api/IdeaTopics/0/NoC/topic
    [HttpDelete("{ideaId}/{catKey}/{topicName}")]
    public async Task<IActionResult> DeleteIdeaTopic(int ideaId, string catKey, string topicName)
    {
        if (_context.IdeaTopic == null) { return NotFound(); }

        catKey = HttpUtility.UrlDecode(catKey);
        topicName = HttpUtility.UrlDecode(topicName);

        var topic = await _context.Topic.FirstOrDefaultAsync(t =>
            t.CategoryKey == catKey && t.Name == topicName);
        if (topic == null) { return NotFound("Specified topic not found."); }

        var ideaTopic = await _context.IdeaTopic.FirstOrDefaultAsync(it =>
            it.IdeaId == ideaId && topic.CategoryKey == catKey && topic.Name == topicName);
        if (ideaTopic == null) { return NotFound("Specified IdeaTopic not found."); }

        _context.IdeaTopic.Remove(ideaTopic);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IdeaTopicExists(int ideaId, Guid topicId)
    {
        return (_context.IdeaTopic?.Any(e =>
            e.IdeaId == ideaId && e.TopicId == topicId
            )).GetValueOrDefault();
    }
}
