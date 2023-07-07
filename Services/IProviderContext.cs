
using OnboardingApp.Model;
using System.Collections.Generic;

namespace OnboardingApp.Services
{
    public interface IProviderContext
    {
        IList<Provider> GetAllUserProviders(string token);
        Provider GetById(int id);
        int Add(Model.Provider provider);
        int Update(int id, Model.Provider provider);
        int Delete(int id);

    }
}