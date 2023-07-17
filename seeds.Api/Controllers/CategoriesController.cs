using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly seedsApiContext _context;

        public CategoriesController(seedsApiContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync()
        {
            if (_context.Category == null)
            {
                return NotFound();
            }
            return await _context.Category.ToListAsync();
        }

        // GET: api/Categories/NoC
        [HttpGet("{categoryKey}")]
        public async Task<ActionResult<Category>> GetCategoryAsync(string categoryKey)
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

            return category;
        }
    }
}
