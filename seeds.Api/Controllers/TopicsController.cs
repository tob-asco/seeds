using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly seedsApiContext context;
    private readonly IMapper mapper;

    public TopicsController(
        seedsApiContext context,
        IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    // GET: api/Topics
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TopicFromDb>>> GetTopics()
    {
        if (context.Topic == null) { return NotFound(); }

        var topics = await context.Topic
            .OrderBy(t => t.CategoryKey)
            .ToListAsync();
        if (topics == null || topics?.Count == 0) { return NotFound(); }

        var topicDtoList = mapper.Map<List<TopicFromDb>>(topics);
        return topicDtoList;
    }

    // GET: api/Topics/Noc/topic
    [HttpGet("{catKey}/{name}")]
    public async Task<ActionResult<TopicFromDb>> GetTopic(string catKey, string name)
    {
        if (context.Topic == null) { return NotFound(); }

        catKey = HttpUtility.UrlDecode(catKey);
        name = HttpUtility.UrlDecode(name);

        var topic = await context.Topic.FirstOrDefaultAsync(t =>
            t.CategoryKey == catKey && t.Name == name);
        if (topic == null) { return NotFound(); }

        var topicDto = mapper.Map<TopicFromDb>(topic);
        return topicDto;
    }
}