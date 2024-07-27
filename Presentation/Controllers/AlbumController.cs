using Application.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        public AlbumController(IAlbumRepository albumRepository)
        {
            AlbumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
        }

        private IAlbumRepository AlbumRepository { get; }

        [HttpPost]
        public async Task<ActionResult<Album>> AddAlbumAsync([FromBody] AlbumBindingModel albumBindingModel, CancellationToken cancellation = default)
        {
            var album = new Album
            {
                Name = albumBindingModel.Name,
                Link = albumBindingModel.Link
            };

            var result = await AlbumRepository.AddAlbumAsync(album, cancellation);

            return Ok(result);
        }
    }
}
