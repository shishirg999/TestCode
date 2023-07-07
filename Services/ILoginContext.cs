
using OnboardingApp.Model;

namespace OnboardingApp.Services
{
    public interface ILoginContext
    {
        Response Login(LoginRequest loginRequest);
        Response Register(RegisterRequest registerRequest);
        void Logout(string token);

    }
}