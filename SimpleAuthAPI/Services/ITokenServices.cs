using SimpleAuthAPI.Entities;

namespace SimpleAuthAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<string> roles);
    }
}
