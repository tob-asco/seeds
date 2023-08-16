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

        // GET: api/UserPreferences
        [HttpGet("{username}")]
        public async Task<ActionResult<List<UserPreference>>> GetPreferencesOfUser(
            string username)
        {
            if(_context.UserPreference != null)
            {
                var usersCups = _context.UserPreference.Where(
                    cup => cup.Username == username);
                if (! await usersCups.AnyAsync()) { return new List<UserPreference>(); }
                return usersCups.ToList();
            }
            return NotFound();
        }

        // POST: api/UserPreferences/upsert
        /// <summary>
        /// Upsert (update + insert) endpoint.
        /// May later be used for all Items that have a Guid PK.
        /// For now a UserPreference is a join entity between User and Tag.
        /// </summary>
        /// <param name="cup">The new or updated preference.</param>
        /// <returns></returns>
        [HttpPost("upsert")]
        public async Task<IActionResult> PostOrPutUserPreference(UserPreference cup)
        {
            try
            {
                if (!CatagUserPreferenceExists(cup.Username, cup.ItemId))
                {
                    //if ((!_context.Tag?.Any(e => e.Id == cup.ItemId))
                    //    .GetValueOrDefault())
                    //{  }
                    // POST
                    _context.UserPreference.Add(cup);
                    await _context.SaveChangesAsync();
                    return Created("GetPreferencesOfUser", cup);
                }
                else
                {
                    // PUT
                    _context.Entry(cup).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex) { return Problem(ex.Message); }
        }

        private bool CatagUserPreferenceExists(string username, Guid itemId)
        {
            return (_context.UserPreference?.Any(e =>
                e.Username == username &&
                e.ItemId == itemId))
                .GetValueOrDefault();
        }
    }
}
