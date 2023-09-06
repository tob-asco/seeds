using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliesController : ControllerBase
    {
        private readonly seedsApiContext context;

        public FamiliesController(seedsApiContext context)
        {
            this.context = context;
        }

        // GET: api/Families
        /// <summary>
        /// Get endpoint for all families, which are part of the DB.
        /// We include the Tags by projection.
        /// Further Columns in Family.cs need to be added here.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Family>>> GetFamilies()
        {
            if (context.Family == null) { return NotFound(); }

            var fams = await context.Family
                .Include(f => f.Tags)
                .Select(f => new Family()
                {
                    /* manually include all columns,
                     * to project only to first layer of navigation properties
                     */
                    Id = f.Id,
                    Name = f.Name,
                    CategoryKey = f.CategoryKey,
                    Tags = f.Tags,
                    ProbablePreference = f.ProbablePreference,
                })
                .ToListAsync();

            if(fams == null || fams.Count == 0) { return NotFound(); }

            return fams;
        }

        private bool FamilyExists(Guid id)
        {
            return (context.Family?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
