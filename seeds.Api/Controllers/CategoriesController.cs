﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly seedsApiContext _context;
        private readonly IMapper mapper;

        public CategoriesController(
            seedsApiContext context,
            IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var categories = await _context.Category.ToListAsync();
            if (categories == null || categories?.Count == 0) { return NotFound(); }
            var categoriesDto = mapper.Map<List<CategoryDto>>(categories);
            return categoriesDto;
        }

        // GET: api/Categories/NoC
        [HttpGet("{categoryKey}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(string categoryKey)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            categoryKey = HttpUtility.UrlDecode(categoryKey);
            var category = await _context.Category.FindAsync(categoryKey);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = mapper.Map<CategoryDto>(category);
            return categoryDto;
        }
    }
}
