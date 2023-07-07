using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnboardingApp.Model
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }
        public IEnumerable<UserProvider> UserProviders { get; set; }
        public IEnumerable<UserAddress> UserAddresses { get; set; }
        public IEnumerable<Conversation> Conversations { get; set; }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }

    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UserProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ProviderId { get; set; }
        public User User { get; set; }
        public Provider Provider { get; set; }
    }
}