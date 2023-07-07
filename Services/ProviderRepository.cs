namespace OnboardingApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using OnboardingApp.Model;
    using System.Collections.Generic;
    using System.Linq;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ProviderRepository : IProviderRepository
    {
        private readonly AppDbContext _context;

        public ProviderRepository(AppDbContext context)
        {
            _context = context;
        }

        public Provider GetById(int id)
        {
            var response = _context.Providers.Where(p => p.ID == id).Include("ProviderAddresses").Include("ProviderAddresses.Address").FirstOrDefault();
            var providerAddress = response.ProviderAddresses.FirstOrDefault();
            if (providerAddress != null)
            {
                response.Street1 = providerAddress.Address.Street1;
                response.Street2 = providerAddress.Address.Street2;
                response.City = providerAddress.Address.City;
                response.Zip = providerAddress.Address.Zip;
            }
            return response;
        }

        public IList<Provider> GetAllUserProviders(int userid)
        {
            var responseData = _context.UserProviders.Where(p => p.UserId == userid)
                .Include("Provider.ProviderAddresses").Include("Provider.ProviderAddresses.Address").ToList();

            IList<Provider> response = null;

            if (responseData.Any())
            {
                response = new List<Provider>();

                foreach (var item in responseData)
                {
                    var provider = new Provider();
                    provider.ID = item.ProviderId;
                    provider.Name = item.Provider.Name;

                    var providerAddress = item.Provider.ProviderAddresses.FirstOrDefault();
                    if (providerAddress != null) {
                        provider.Street1 = providerAddress.Address.Street1;
                        provider.Street2 = providerAddress.Address.Street2;
                        provider.City = providerAddress.Address.City;
                        provider.Zip = providerAddress.Address.Zip;
                    }
                    response.Add(provider);
                }
            }

            return response;
        }

        public void Add(Provider provider)
        {
            _context.Providers.Add(provider);
            _context.SaveChanges();
        }

        public void Update(Provider provider)
        {
            _context.Providers.Update(provider);
            _context.SaveChanges();
        }

        public void Delete(Provider provider)
        {
            _context.Providers.Remove(provider);
            _context.SaveChanges();
        }

       

    }
}