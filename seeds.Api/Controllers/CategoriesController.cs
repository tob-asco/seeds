using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

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
        public async Task<ActionResult<IEnumerable<CategoryFromDb>>> GetCategories()
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var categories = await _context.Category.ToListAsync();
            if (categories == null || categories?.Count == 0) { return NotFound(); }
            var categoriesDto = mapper.Map<List<CategoryFromDb>>(categories);
            return categoriesDto;
        }

        // GET: api/Categories/NoC
        [HttpGet("{categoryKey}")]
        public async Task<ActionResult<CategoryFromDb>> GetCategory(string categoryKey)
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var category = await _context.Category.FindAsync(categoryKey);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = mapper.Map<CategoryFromDb>(category);
            return categoryDto;
        }
    }
}
