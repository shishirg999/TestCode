using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingApp
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class JwtConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryDays { get; set; }
        public string Key { get; set; }
    }
}
