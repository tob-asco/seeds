using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.ToApi;
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
        public async Task<ActionResult<IEnumerable<CategoryDtoApi>>> GetCategories()
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            var categories = await _context.Category.ToListAsync();
            var categoriesDto = mapper.Map<List<CategoryDtoApi>>(categories);
            return categoriesDto;
        }

        // GET: api/Categories/NoC
        [HttpGet("{categoryKey}")]
        public async Task<ActionResult<CategoryDtoApi>> GetCategory(string categoryKey)
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
            var categoryDto = mapper.Map<CategoryDtoApi>(category);
            return categoryDto;
        }
    }
}
