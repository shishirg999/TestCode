using Model = OnboardingApp.Model;
using System.Collections.Generic;

namespace OnboardingApp.Services
{
    public interface IProviderRepository
    {
        Model.Provider GetById(int id);
        IList<Model.Provider> GetAllUserProviders(int userid);
        void Add(Model.Provider provider);
        void Update(Model.Provider provider);
        void Delete(Model.Provider provider);
        
    }
}