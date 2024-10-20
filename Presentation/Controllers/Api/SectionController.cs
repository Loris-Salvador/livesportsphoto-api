﻿using Application.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers.Api
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


        [HttpGet]
        public async Task<ActionResult<List<Section>>> ToListAsync(CancellationToken cancellation)
        {
            var sections = await SectionRepository.ToListAsync(cancellation);

            return Ok(sections);
        }


        [HttpGet("albums/{sectionId}")]
        public async Task<ActionResult<List<Album>>> GetAlbums(string sectionId, CancellationToken cancellation)
        {
            var albums = await SectionRepository.GetAlbumsAsync(sectionId, cancellation);

            return Ok(albums);
        }


        [HttpPost]
        public async Task<ActionResult<Section>> AddAsync(string name, CancellationToken cancellation)
        {
            var section = new Section()
            {
                Name = name,
            };

            var result = await SectionRepository.AddAsync(section, cancellation);

            return Ok(result);
        }


        [HttpPost("album")]
        public async Task<ActionResult<Album>> AddAlbumAsync(string sectionId, [FromBody] AlbumBindingModel albumBindingModel, CancellationToken cancellation)
        {
            var album = new Album
            {
                Name = albumBindingModel.Name,
                Link = albumBindingModel.Link
            };

            var result = await SectionRepository.AddAlbumAsync(sectionId, album, cancellation);

            return Ok(result);
        }


        [HttpDelete()]
        public async Task<ActionResult<Section>> DeleteAsync(string sectionId, CancellationToken cancellation)
        {
            var sectionDelete = await SectionRepository.DeleteAsync(sectionId, cancellation);

            return Ok(sectionDelete);
        }


        [HttpDelete("album")]
        public async Task<ActionResult<Album>> DeleteAlbumAsync(string sectionId, string albumId, CancellationToken cancellation)
        {
            var albumDeleted = await SectionRepository.DeleteAlbumAsync(sectionId, albumId, cancellation);

            return Ok(albumDeleted);
        }
    }
}
