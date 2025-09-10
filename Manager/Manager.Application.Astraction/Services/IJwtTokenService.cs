using Manager.Doman.Entites;

namespace Manager.Application.Astraction.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
