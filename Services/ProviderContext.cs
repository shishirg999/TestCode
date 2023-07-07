namespace OnboardingApp.Services
{
    using OnboardingApp.Model;
    // Purpose: Class for handling onboarding requests
    using System;
    using System.Collections.Generic;

    public class ProviderContext : IProviderContext
    {
        private readonly IProviderRepository _providerRepository;
        private readonly ITokenValidator _tokenValidator;

        public ProviderContext(IProviderRepository providerRepository, ITokenValidator tokenValidator)
        {
            _providerRepository = providerRepository;
            _tokenValidator = tokenValidator;
        }

        public IList<Provider> GetAllUserProviders(string token)
        {
            var user = _tokenValidator.ValidateToken(token);
            return _providerRepository.GetAllUserProviders(user.ID);
        }

        public Provider GetById(int id)
        {
            return _providerRepository.GetById(id);
        }

        public int Add(Provider provider)
        {
            _providerRepository.Add(provider);
            return provider.ID;
        }

        public int Update(int id, Provider provider)
        {
            var existingProvider = _providerRepository.GetById(id);

            if (existingProvider == null)
            {
                throw new Exception("Provider not found");
            }

            existingProvider.Name = provider.Name;
            existingProvider.NPI = provider.NPI;

            _providerRepository.Update(existingProvider);
            return existingProvider.ID;
        }

        public int Delete(int id)
        {
            var existingProvider = _providerRepository.GetById(id);

            if (existingProvider == null)
            {
                throw new Exception("Provider not found");
            }

            _providerRepository.Delete(existingProvider);
            return existingProvider.ID;
        }
        
    }
}