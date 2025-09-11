using Manager.Application.Astraction.Services;


namespace Manager.Application.Services
{
    public class PasswordValidatorService : IPasswordValidatorService
    {

        private const int MinLength = 8;
        private const int MaxLength = 64;

        public bool IsValid(string password)
        {
            return !GetValidationErrors(password).Any();
        }

        public IEnumerable<string> GetValidationErrors(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                yield return "Password cannot be empty.";

            if (password.Length < MinLength)
                yield return $"Password must be at least {MinLength} characters long.";

            if (password.Length > MaxLength)
                yield return $"Password cannot exceed {MaxLength} characters.";

            if (!password.Any(char.IsUpper))
                yield return "Password must contain at least one uppercase letter.";

            if (!password.Any(char.IsLower))
                yield return "Password must contain at least one lowercase letter.";

            if (!password.Any(char.IsDigit))
                yield return "Password must contain at least one digit.";

            if (!password.Any(c => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c)))
                yield return "Password must contain at least one special character.";
        }
    }
}
