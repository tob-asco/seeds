using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferencesController : ControllerBase
    {
        private readonly seedsApiContext _context;
        private readonly IMapper mapper;

        public UserPreferencesController(
            seedsApiContext context,
            IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/UserPreferences/tobi
        [HttpGet("{username}")]
        public async Task<ActionResult<List<UserPreference>>> GetPreferencesOfUser(
            string username)
        {
            if (_context.UserPreference != null)
            {
                username = HttpUtility.UrlDecode(username);
                var usersCups = _context.UserPreference.Where(
                    cup => cup.Username == username);
                return usersCups != null ?
                    await usersCups.ToListAsync() : new List<UserPreference>();
            }
            return NotFound();
        }

        // GET: api/UserPreferences/orphans?username=tobi
        /// <summary>
        /// Usage: When constructing the PreferencesPage, exactly
        /// these tags will get their own button.
        /// </summary>
        /// <param name="username">CurrentUser.Username</param>
        /// <returns>A list of all tags that either have no family
        /// or that have a family and a CurrentUser's preference (!= 0)</returns>
        [HttpGet("buttonedTags")]
        public async Task<ActionResult<List<TagFromDb>>> GetButtonedTags(
            string username = "")
        {
            username = HttpUtility.UrlDecode(username);
            var orphans = _context.Tag.Where(t => t.FamilyId == null);
            if (orphans == null || !await orphans.AnyAsync())
            { return NotFound("The tags with no family."); }

            var orphansDto = mapper.Map<List<TagFromDb>>(
                await orphans.ToListAsync());

            if (username == "") { return orphansDto; }

            // get tags that have family and preference (also orphans)
            var tagsWithFamilyAndUserPreference = await _context.Tag
                .Where(tag => tag.FamilyId != null)
                .Where(tag => _context.UserPreference.Any(
                    up => up.ItemId == tag.Id
                       && up.Value != 0
                       && up.Username == username))
                .ToListAsync();
            if(tagsWithFamilyAndUserPreference == null)
            { return NotFound("The tags in a family with non-trivial preference."); }

            return orphansDto.Concat(
                mapper.Map<List<TagFromDb>>(tagsWithFamilyAndUserPreference))
                .ToList();
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
