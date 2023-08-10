using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;
using System.Web;

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
        [HttpGet("{catKey}/{username}")]
        public async Task<ActionResult<CategoryUserPreference>> GetCategoryUserPreference(
            string catKey, string username, string? tagName)
        {
            catKey = HttpUtility.UrlDecode(catKey);
            username = HttpUtility.UrlDecode(username);
            tagName = HttpUtility.UrlDecode(tagName);
            try
            {
                var categoryUserPreference = await _context.CategoryUserPreference
                    .FirstOrDefaultAsync(cup =>
                    cup.CategoryKey == catKey &&
                    cup.Username == username &&
                    cup.TagName == tagName); // test that a null tagName does what you want
                return categoryUserPreference != null ? categoryUserPreference : NotFound();
            }
            catch (Exception ex) { return Problem(ex.Message); }
        }

        // PUT: api/CategoryUserPreferences/NoC/tobi?tagName=tag
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{catKey}/{username}")]
        public async Task<IActionResult> PutCategoryUserPreference(
            string catKey, string username, string? tagName,
            CategoryUserPreference cup)
        {
            catKey = HttpUtility.UrlDecode(catKey);
            username = HttpUtility.UrlDecode(username);
            tagName = HttpUtility.UrlDecode(tagName);
            if (catKey != cup.CategoryKey
                || username != cup.Username) { return BadRequest("Inconsistent request."); }

            // we use that the triple (cup.CategoryKey, cup.Username, cup.TagName) is unique!
            var oldCup = await _context.CategoryUserPreference.FirstOrDefaultAsync(e =>
                e.CategoryKey == catKey &&
                e.Username == username &&
                e.TagName == tagName);

            if (oldCup != null)
            {
                // oldCup is part of the Change Tracker so we can simply change it and
                // this will affect the _context.
                oldCup.Value = cup.Value;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) { return Problem(ex.Message); }
            }
            else { return NotFound(); }

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
