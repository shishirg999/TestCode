namespace OnboardingApp.Services
{
    // Purpose: Generates a JWT token for the user to use for authentication
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using OnboardingApp.Model;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("Id", user.ID.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateSignature(string header, string payload)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var signature = $"{header}.{payload}";
            var bytes = Encoding.UTF8.GetBytes(signature);
            var hash = new HMACSHA256(key);
            var computedHash = hash.ComputeHash(bytes);
            return Convert.ToBase64String(computedHash);
        }

        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}