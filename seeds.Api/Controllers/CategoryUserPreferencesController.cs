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

        // GET: api/CategoryUserPreferences/tobi/NoC
        [HttpGet("{username}/{categoryKey}")]
        public async Task<ActionResult<CategoryUserPreference>> GetCategoryUserPreferenceAsync(string username, string categoryKey)
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

        // PUT: api/CategoryUserPreferences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryUserPreferenceAsync(string id, CategoryUserPreference categoryUserPreference)
        {
            if (id != categoryUserPreference.CategoryKey)
            {
                return BadRequest();
            }

            _context.Entry(categoryUserPreference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryUserPreferenceExists(id))
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

        // POST: api/CategoryUserPreferences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryUserPreference>> PostCategoryUserPreferenceAsync(CategoryUserPreference categoryUserPreference)
        {
          if (_context.CategoryUserPreference == null)
          {
              return Problem("Entity set 'seedsApiContext.CategoryUserPreference'  is null.");
          }
            _context.CategoryUserPreference.Add(categoryUserPreference);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoryUserPreferenceExists(categoryUserPreference.CategoryKey))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCategoryUserPreference", new { id = categoryUserPreference.CategoryKey }, categoryUserPreference);
        }

        // DELETE: api/CategoryUserPreferences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryUserPreferenceAsync(string id)
        {
            if (_context.CategoryUserPreference == null)
            {
                return NotFound();
            }
            var categoryUserPreference = await _context.CategoryUserPreference.FindAsync(id);
            if (categoryUserPreference == null)
            {
                return NotFound();
            }

            _context.CategoryUserPreference.Remove(categoryUserPreference);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryUserPreferenceExists(string id)
        {
            return (_context.CategoryUserPreference?.Any(e => e.CategoryKey == id)).GetValueOrDefault();
        }
    }
}
