using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface ITokenRepository
{
    Task<Token> GetById(int id);
    Task<bool> Update(Token token);
}
