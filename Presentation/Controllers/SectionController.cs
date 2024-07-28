using Application.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        public SectionController(ISectionRepository sectionRepository)
        {
            SectionRepository = sectionRepository ?? throw new ArgumentNullException(nameof(sectionRepository));
        }

        private ISectionRepository SectionRepository { get; }

        [HttpPost]
        public async Task<ActionResult<Section>> AddAlbumAsync(string name, CancellationToken cancellation = default)
        {
            var section = new Section()
            {
                Name = name,
            };

            var result = await SectionRepository.AddAsync(section, cancellation);

            return Ok(result);
        }
    }
}
