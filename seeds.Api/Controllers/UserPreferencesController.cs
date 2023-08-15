using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferencesController : ControllerBase
    {
        private readonly seedsApiContext _context;

        public UserPreferencesController(seedsApiContext context)
        {
            _context = context;
        }

        // GET: api/CatagUserPreferences/NoC/tobi?tagName=tag
        [HttpGet("{catKey}/{username}")]
        public async Task<ActionResult<UserPreference>> GetCatagUserPreference(
            string catKey, string username, string? tagName)
        {
            catKey = HttpUtility.UrlDecode(catKey);
            username = HttpUtility.UrlDecode(username);
            tagName = HttpUtility.UrlDecode(tagName);
            try
            {
                var CatagUserPreference = await _context.CatagUserPreference
                    .FirstOrDefaultAsync(cup =>
                    cup.CategoryKey == catKey &&
                    cup.Username == username &&
                    cup.TagName == tagName); // test that a null tagName does what you want
                return CatagUserPreference != null ? CatagUserPreference : NotFound();
            }
            catch (Exception ex) { return Problem(ex.Message); }
        }

        // PUT: api/CatagUserPreferences/NoC/tobi?tagName=tag
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{catKey}/{username}")]
        public async Task<IActionResult> PutCatagUserPreference(
            string catKey, string username, string? tagName,
            UserPreference cup)
        {
            catKey = HttpUtility.UrlDecode(catKey);
            username = HttpUtility.UrlDecode(username);
            tagName = HttpUtility.UrlDecode(tagName);
            if (catKey != cup.CategoryKey
                || username != cup.Username) { return BadRequest("Inconsistent request."); }

            // we use that the triple (cup.CategoryKey, cup.Username, cup.TagName) is unique!
            var oldCup = await _context.CatagUserPreference.FirstOrDefaultAsync(e =>
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

        private bool CatagUserPreferenceExists(string categoryKey, string username, string? tagName)
        {
            return (_context.CatagUserPreference?.Any(e =>
                e.CategoryKey == categoryKey &&
                e.Username == username &&
                e.TagName == tagName))
                .GetValueOrDefault();
        }
    }
}
