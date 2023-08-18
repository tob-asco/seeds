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
        private readonly seedsApiContext _context;

        public FamiliesController(seedsApiContext context)
        {
            _context = context;
        }

        // GET: api/Families
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Family>>> GetFamilies()
        {
            if (_context.Family == null)
            {
                return NotFound();
            }
            var fams = await _context.Family.ToListAsync();
            if(fams == null || fams.Count == 0) { return NotFound(); }
            return fams;
        }

        private bool FamilyExists(Guid id)
        {
            return (_context.Family?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
