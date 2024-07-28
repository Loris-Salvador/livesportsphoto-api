using Domain.Models;

namespace Application.Repositories;

public interface ISectionRepository
{
    Task<Section> AddAsync(Section section, CancellationToken cancellationToken);
}