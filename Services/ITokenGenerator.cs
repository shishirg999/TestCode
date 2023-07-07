using OnboardingApp.Model;

namespace OnboardingApp.Services
{
    public interface ITokenGenerator
    {
        public string GenerateToken(User user);
        public string GenerateSignature(string header, string payload);
        public int? ValidateToken(string token);
    }
}