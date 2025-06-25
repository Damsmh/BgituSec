using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BgituSec.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTConfig _jwt;
        private readonly byte[] _key;
        private readonly int _saltBytes;
        private readonly IMapper _mapper;

        public TokenService(IOptions<JWTConfig> options, IMapper mapper)
        {
            _jwt = options.Value;
            _key = Encoding.UTF8.GetBytes(_jwt.Key);
            _saltBytes = _jwt.Bytes;
            _mapper = mapper;
        }

        public string CreateToken(UserDTO user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullName", user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiresMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshTokenDTO GenerateRefreshToken(int userId)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var newRefreshToken = Convert.ToBase64String(randomNumber);
            RefreshToken Token = new()
            {
                Token = newRefreshToken,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            var RefreshToken = _mapper.Map<RefreshTokenDTO>(Token);
            return RefreshToken;
        }

        public string Hash(string token)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(_saltBytes);
            var hashedToken = BCrypt.Net.BCrypt.HashPassword(token, salt);
            return hashedToken;
        }

        public bool Verify(string newToken, string oldHash)
        {
            return BCrypt.Net.BCrypt.Verify(newToken, oldHash);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwt.Issuer,
                ValidAudience = _jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(_key)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            return principal;


        }
    }
}
