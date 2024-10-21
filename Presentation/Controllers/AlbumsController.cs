using Application.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Controllers
{
    public class AlbumsController : Controller
    {
        public AlbumsController(ISectionRepository sectionRepository)
        {
            SectionRepository = sectionRepository ?? throw new ArgumentNullException(nameof(sectionRepository));
        }

        private ISectionRepository SectionRepository { get; }

        public async Task<IActionResult> Index(string id, CancellationToken cancellationToken)
        {
            var albums = await SectionRepository.GetAlbumsAsync(id, cancellationToken);

            if (albums.IsNullOrEmpty())
            {
                return NotFound();
            }

            var section = new Section
            {
                Name = id,
                Albums = albums
            };

            return View(section);
        }


    }
}
