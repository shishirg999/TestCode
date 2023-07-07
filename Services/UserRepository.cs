namespace OnboardingApp.Services
{
    using OnboardingApp.Model;
    // Purpose: Contains logic for interacting with the User table in the database.
    using System.Linq;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.ID == id);
        }

        public int Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.ID;
        }

        public int Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user.ID;
        }

        public int Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
            return user.ID;
        }

    }
}