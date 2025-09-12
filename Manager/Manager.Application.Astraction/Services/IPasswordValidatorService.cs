
namespace Manager.Application.Astraction.Services
{
    public interface IPasswordValidatorService
    {
        bool IsValid(string password);
        IEnumerable<string> GetValidationErrors(string password);
    }
}
