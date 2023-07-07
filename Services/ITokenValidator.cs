using OnboardingApp.Model;

namespace OnboardingApp.Services
{
    public interface ITokenValidator
    {
        bool Validate(string token);
        User ValidateToken(string token);
    }
}