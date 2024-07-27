using Domain.Models;

namespace Application.Repositories;

public interface IAlbumRepository
{
    Task<Album> AddAlbumAsync(Album album, CancellationToken cancellationToken = default);
}