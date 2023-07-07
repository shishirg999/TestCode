using OnboardingApp.Model;

namespace OnboardingApp.Services
{
    public interface IUserRepository
    {
        User GetByEmail(string email);
        User GetById(int id);
        int Add(User user);
        int Update(User user);
        int Delete(User user);

    }
}