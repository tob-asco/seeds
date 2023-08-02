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

        // GET: api/CategoryUserPreferences/NoC/tobi?tagName=tag
        [HttpGet("{categoryKey}/{username}")]
        public async Task<ActionResult<CategoryUserPreference>> GetCategoryUserPreference(
            string categoryKey, string username, string? tagName)
        {
            try
            {
                var categoryUserPreference = await _context.CategoryUserPreference
                    .FirstAsync(cup =>
                    cup.CategoryKey == categoryKey &&
                    cup.Username == username &&
                    cup.TagName == tagName); // test that a null tagName does what you want
                return categoryUserPreference != null ? categoryUserPreference : NotFound();
            }
            catch (Exception ex) { return Problem(ex.Message); }
        }

        // PUT: api/CategoryUserPreferences/NoC/tobi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{categoryKey}/{username}")]
        public async Task<IActionResult> PutCategoryUserPreference(
            string categoryKey, string username, string? tagName,
            CategoryUserPreference cup)
        {
            if (categoryKey != cup.CategoryKey
                || username != cup.Username) { return BadRequest(); }

            _context.Entry(cup).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryUserPreferenceExists(categoryKey, username, tagName))
                {
                    return NotFound();
                }
                else { throw; }
            }
            return NoContent();
        }

        private bool CategoryUserPreferenceExists(string categoryKey, string username, string? tagName)
        {
            return (_context.CategoryUserPreference?.Any(e =>
                e.CategoryKey == categoryKey && 
                e.Username == username &&
                e.TagName == tagName))
                .GetValueOrDefault();
        }
    }
}
