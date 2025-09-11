using Manager.Application.Astraction.Services;

namespace Manager.Application.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
