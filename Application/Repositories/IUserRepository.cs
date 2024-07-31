using Domain.Models;

namespace Application.Repositories;

public interface IUserRepository
{
    Task<User> GetUser(string name, CancellationToken cancellationToken = default);
}