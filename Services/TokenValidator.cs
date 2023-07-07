namespace OnboardingApp.Services
{
    using Newtonsoft.Json;
    using OnboardingApp.Model;
    // Purpose: Class for validating user tokens.
    using System;
    using System.Security.Claims;
    using System.Text;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TokenValidator : ITokenValidator
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public TokenValidator(ITokenGenerator tokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public bool Validate(string token)
        {
            var tokenParts = token.Split('.');
            if (tokenParts.Length != 3)
            {
                return false;
            }

            var header = tokenParts[0];
            var tokenPayload = tokenParts[1];
            var tokenSignature = tokenParts[2];

            var payloadBytes = Convert.FromBase64String(tokenPayload);
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<TokenPayload>(payloadJson);

            var user = _userRepository.GetById(payload.Id);
            if (user == null)
            {
                return false;
            }

            var signature = _tokenGenerator.GenerateSignature(header, tokenPayload);
            return signature == tokenSignature;


        }

        public User ValidateToken(string token)
        {
            var response = _tokenGenerator.ValidateToken(token);
            if (!response.HasValue)
            {
                throw new Exception("Invalid token");
            }

            return _userRepository.GetById(response.Value);
        }
    }
}