using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User[]> GetAll();
    Task<User> GetById(int id);
}
