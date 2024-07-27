using Domain.Models;

namespace Application.Repositories;

public interface IAlbumRepository
{
    Task AddAlbumAsync(Album album);
}