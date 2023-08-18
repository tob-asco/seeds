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
public class TagsController : ControllerBase
{
    private readonly seedsApiContext context;
    private readonly IMapper mapper;

    public TagsController(
        seedsApiContext context,
        IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    // GET: api/Tags
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagFromDb>>> GetTags()
    {
        if (context.Tag == null) { return NotFound(); }

        var tags = await context.Tag
            .OrderBy(t => t.CategoryKey)
            .ToListAsync();
        if (tags == null || tags?.Count == 0) { return NotFound(); }

        var tagDtoList = mapper.Map<List<TagFromDb>>(tags);
        return tagDtoList;
    }

    // GET: api/Tags/Noc/tag
    [HttpGet("{catKey}/{name}")]
    public async Task<ActionResult<TagFromDb>> GetTag(string catKey, string name)
    {
        if (context.Tag == null) { return NotFound(); }

        catKey = HttpUtility.UrlDecode(catKey);
        name = HttpUtility.UrlDecode(name);

        var tag = await context.Tag.FirstOrDefaultAsync(t =>
            t.CategoryKey == catKey && t.Name == name);
        if (tag == null) { return NotFound(); }

        var tagDto = mapper.Map<TagFromDb>(tag);
        return tagDto;
    }
}