using Manager.Domain.Entites;

namespace Manager.Application.Astraction.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
