namespace OnboardingApp.Services
{
    using OnboardingApp.Model;
    // Purpose: Class for handling onboarding requests
    using System;
    public class LoginContext : ILoginContext
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenValidator _tokenValidator;
        private readonly IUserRepository _userRepository;

        public LoginContext(IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, ITokenValidator tokenValidator, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _userRepository = userRepository;
        }

        public Response Login(LoginRequest loginRequest)
        {
            var user = _userRepository.GetByEmail(loginRequest.Email);
            if (user == null)
            {
                return new Response(404, "User not found", "");
            }

            if (!_passwordHasher.VerifyPassword(loginRequest.Password, user.Password))
            {
                return new Response(401, "Invalid password", "");
            }

            user.Token = _tokenGenerator.GenerateToken(user);
            return new Response(200, "Ok", user.Token);
        }

        public Response Register(RegisterRequest registerRequest)
        {
            var user = _userRepository.GetByEmail(registerRequest.Email);
            if (user != null)
            {
                return new Response(409, "User already exists", "");
            }

            user = new User
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = _passwordHasher.HashPassword(registerRequest.Password)
            };

            var userId = _userRepository.Add(user);
            if (userId > 0)
            {
                user.ID = userId;
                user.Token = _tokenGenerator.GenerateToken(user);
                return new Response(200, "Ok", user.Token);
            }

            return new Response(501, "Internal Server Error", "");
        }

        public void Logout(string token)
        {
            var user = _tokenValidator.ValidateToken(token);
            user.Token = null;
            //_userRepository.Update(user);
        }
    }
}