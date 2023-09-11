using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Data;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliesController : ControllerBase
    {
        private readonly seedsApiContext context;
        private readonly IMapper mapper;

        public FamiliesController(
            seedsApiContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Families
        /// <summary>
        /// Get endpoint for all families, which are part of the DB.
        /// We include the Topics by projection.
        /// Further Columns in Family.cs need to be added here.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<FamilyFromDb>> GetFamilies()
        {
            if (context.Family == null) { return NotFound(); }

            var fams = context.Family
                .Include(f => f.Topics)
                .AsEnumerable()
                .Select(f =>
                {
                    /* manually include all columns,
                     * to project only to first layer of navigation properties
                     */
                    Family fCopy = f.ShallowCopy();
                    fCopy.Topics = f.Topics.OrderBy(t => t.Name).ToList();
                    return fCopy;
                })
                .ToList();

            if(fams == null || fams.Count == 0) { return NotFound(); }

            var famsDto = mapper.Map<List<FamilyFromDb>>(fams);
            return famsDto;
        }

        private bool FamilyExists(Guid id)
        {
            return (context.Family?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
