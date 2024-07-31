using Domain.Models;

namespace Application.Repositories;

public interface ISectionRepository
{
    Task<Section> AddAsync(Section section, CancellationToken cancellationToken = default);

    Task<Album> AddAlbumAsync(string sectionId, Album album, CancellationToken cancellationToken = default);

    Task<List<Section>> ToListAsync(CancellationToken cancellationToken = default);

    Task<Album> DeleteAlbumAsync(string sectionId, string albumId, CancellationToken cancellationToken = default);

    Task<List<Album>> GetAlbumAsync(string sectionId, CancellationToken cancellationToken = default);

    Task<Section> DeleteAsync(string sectionId, CancellationToken cancellationToken = default);
}