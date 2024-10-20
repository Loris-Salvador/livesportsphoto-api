using Application.Repositories;
using Microsoft.AspNetCore.Mvc;

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

            return View(albums);
        }


    }
}
