namespace OnboardingApp.Services
{
    // Purpose: Contains logic for hashing and verifying passwords.
    using BCrypt.Net;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string loginpass, string password)
        {
            string hash = BCrypt.HashPassword(loginpass);
            return BCrypt.Verify(password, hash);
        }
    }
}