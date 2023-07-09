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
    public class CategoryUserPreferencesController : ControllerBase
    {
        private readonly seedsApiContext _context;

        public CategoryUserPreferencesController(seedsApiContext context)
        {
            _context = context;
        }

        // GET: api/CategoryUserPreferences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryUserPreference>>> GetCategoryUserPreferencesAsync()
        {
          if (_context.CategoryUserPreference == null)
          {
              return NotFound();
          }
            return await _context.CategoryUserPreference.ToListAsync();
        }

        // GET: api/CategoryUserPreferences/NoC/tobi
        [HttpGet("{categoryKey}/{username}")]
        public async Task<ActionResult<CategoryUserPreference>> GetCategoryUserPreferenceAsync(string categoryKey, string username)
        {
            try
            {
                var categoryUserPreference = await _context.CategoryUserPreference.FindAsync(categoryKey, username);
                return categoryUserPreference != null ? categoryUserPreference : NotFound();
            }
            catch
            {
                return NotFound();
            }
        }

        // PUT: api/CategoryUserPreferences/NoC/tobi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{categoryKey}/{username}")]
        public async Task<IActionResult> PutCategoryUserPreferenceAsync(
            string categoryKey,
            string username,
            CategoryUserPreference cup)
        {
            if (categoryKey != cup.CategoryKey
                || username != cup.Username)
            {
                return BadRequest();
            }

            _context.Entry(cup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryUserPreferenceExists(categoryKey, username))
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

        private bool CategoryUserPreferenceExists(string categoryKey, string username)
        {
            return (_context.CategoryUserPreference?.Any(e =>
                e.CategoryKey == categoryKey && e.Username == username))
                .GetValueOrDefault();
        }
    }
}
