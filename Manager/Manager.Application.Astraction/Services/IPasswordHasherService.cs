
namespace Manager.Application.Astraction.Services
{
    public interface IPasswordHasherService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string password);
    }
}
