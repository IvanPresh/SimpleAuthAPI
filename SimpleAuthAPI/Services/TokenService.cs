using Microsoft.IdentityModel.Tokens;
using SimpleAuthAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleAuthAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generate a JWT token for a user including their roles.
        /// </summary>
        /// <param name="user">The user for whom the token is being generated.</param>
        /// <param name="roles">The roles assigned to the user.</param>
        /// <returns>A JWT token as a string.</returns>
        public string GenerateToken(User user, List<string> roles)
        {
            try
            {
                TimeSpan? expiration = null;
                var claims = CreateJwtClaims(user, roles); // Create claims for the token
                var options = GetOptions(); 
                var now = DateTime.UtcNow;

                // Create a new JWT security token
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: options.Issuer,
                    audience: options.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(expiration ?? options.Expiration),
                    signingCredentials: options.SigningCredentials
                );

                // Write and return the token as a string
                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating token: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Generate user claims including roles.
        /// </summary>
        /// <param name="user">The user for whom claims are being created.</param>
        /// <param name="roles">The roles assigned to the user.</param>
        /// <returns>A list of claims for the JWT token.</returns>
        private List<Claim> CreateJwtClaims(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                // Add standard claims
                new Claim(JwtRegisteredClaimNames.NameId, "029261DF-0E82-4E5E-9AA8-036606F70ECF"), // Unique identifier
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Subject (user email)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Issued at
                // Add custom claims
                new Claim("email", user.Email),
                new Claim("phoneNumber", user.PhoneNumber),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        // Get token generation options from configuration
        private TokenProviderOptions GetOptions()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:JwtBearer:SecretKey").Value));

            return new TokenProviderOptions
            {
                Audience = _configuration.GetSection("Authentication:JwtBearer:Audience").Value,
                Issuer = _configuration.GetSection("Authentication:JwtBearer:Issuer").Value,
                Expiration = TimeSpan.FromMinutes(Convert.ToInt32(_configuration.GetSection("Authentication:JwtBearer:AccessExpiration").Value)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512)
            };
        }
    }

    // Configuration options for token generation
    public class TokenProviderOptions
    {
        public SymmetricSecurityKey SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public TimeSpan Expiration { get; set; }
    }
}
