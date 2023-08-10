﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;

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
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
    {
        if (context.Tag == null) { return NotFound(); }

        var tags = await context.Tag.ToListAsync();
        if (tags == null || tags?.Count == 0) { return NotFound(); }

        var tagDtoList = mapper.Map<List<TagDto>>(tags);
        return tagDtoList;
    }

    // GET: api/Tags/Noc/tag
    [HttpGet("{catKey}/{name}")]
    public async Task<ActionResult<TagDto>> GetTag(string catKey, string name)
    {
        if (context.Tag == null) { return NotFound(); }

        var tag = await context.Tag.FindAsync(catKey, name);
        if (tag == null) { return NotFound(); }

        var tagDto = mapper.Map<TagDto>(tag);
        return tagDto;
    }
}