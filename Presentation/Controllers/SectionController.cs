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
        public async Task<ActionResult<Section>> AddAsync(string name, CancellationToken cancellation = default)
        {
            var section = new Section()
            {
                Name = name,
            };

            var result = await SectionRepository.AddAsync(section, cancellation);

            return Ok(result);
        }

        [HttpPost("album")]
        public async Task<ActionResult<Album>> AddAlbumAsync(string sectionId, [FromBody] AlbumBindingModel albumBindingModel, CancellationToken cancellation = default)
        {
            var album = new Album
            {
                Name = albumBindingModel.Name,
                Link = albumBindingModel.Link
            };

            var result = await SectionRepository.AddAlbumAsync(sectionId, album, cancellation);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Section>>> ToList(CancellationToken cancellation = default)
        {
            var sections = await SectionRepository.ToList(cancellation);

            return Ok(sections);
        }
    }
}
