using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnboardingApp.Model
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Provider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string NPI { get; set; }
        [NotMapped]
        public string Specialty { get; set; } = "PCP";
        [NotMapped]
        public string ImageURL { get { return RandomImages(); } }
        [NotMapped]
        public string Street1 { get; set; }
        [NotMapped]
        public string Street2 { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string Zip { get; set; }
        [JsonIgnore]
        public IEnumerable<Conversation> Conversations { get; set; }
        [JsonIgnore]
        public IEnumerable<ProviderAddress> ProviderAddresses { get; set; }
        [JsonIgnore]
        public IEnumerable<UserProvider> UserProviders { get; set; }

        private string RandomImages()
        {
            Random Random = new Random();
            int RandomNumber = Random.Next(1, 3);
            string[] Images = new string[] { 
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRYQ8KMBmRhSLSr7MQdFwToaGs-9_907sye9DTOWnvkMw&s",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQzA2p12P1URW3LPX-I7HhIFrLl42lTWHwZXT2VXDcVsg&s",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSI1j4k50y4klj6CwzDzZNXUX8qDYx8niBKyi0ePAZp&s" };
                
            return Images[RandomNumber];
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ProviderAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ProviderId { get; set; }
        public int AddressId { get; set; }
        public Provider Provider { get; set; }
        public Address Address { get; set; }
    }
}