using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Talabat.Domain.Models;
using Talabat.Domain.Services;
using Talabat.Domain.Shared;
using Talabat.Domain.Shared.Constants;
using Talabat.Domain.Shared.Options;

namespace Talabat.Infrastructure.Services
{
    internal class AuthService(
        UserManager<User> _userManager,
        IOptions<JwtOptions> jwtOptions
        ) : IAuthService
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public async Task<object> GenerateAccessTokenAsync(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim(Claims.Role, role));

            var claims = new List<Claim>
            {
                new(Claims.UserId, user.Id),
                new(Claims.UserName, user.UserName!),
                new(Claims.FirstName, user.FirstName!),
                new(Claims.LastName, user.LastName!),
                new(Claims.Email, user.Email!),
                new(Claims.Phone, user.PhoneNumber ?? ""),
            }
            .Union(userClaims)
            .Union(roleClaims);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken
                (
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.ToLocalTime().AddDays(_jwtOptions.ExpirationInDays),
                signingCredentials: signingCredentials
                );
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber).Replace('+', '%'),
                ExpirationOn = DateTime.UtcNow.ToLocalTime().AddDays(7),
                CreatedOn = DateTime.UtcNow.ToLocalTime(),
            };
        }

        public async Task<AuthenticationResponse> GenerateAuthResultAsync(User user)
        {
            var authenticationResponse = new AuthenticationResponse();
            var jwtToken = (JwtSecurityToken)await GenerateAccessTokenAsync(user);

            authenticationResponse.Message = user.UserName!;
            authenticationResponse.UserName = user.UserName!;
            authenticationResponse.Email = user.Email!;
            authenticationResponse.Roles = [.. await _userManager.GetRolesAsync(user)];
            authenticationResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authenticationResponse.TokenExpiration = jwtToken.ValidTo.ToLocalTime();

            if (user.RefreshTokens?.Any(r => r.IsActive) is true)
            {
                var activeRefreshToken = user.RefreshTokens.Single(r => r.IsActive);
                authenticationResponse.RefreshToken = activeRefreshToken.Token;
                authenticationResponse.RefreshTokenExpiration = activeRefreshToken.ExpirationOn.ToLocalTime();
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                user.RefreshTokens?.Add(refreshToken);
                await _userManager.UpdateAsync(user);

                authenticationResponse.RefreshToken = refreshToken.Token;
                authenticationResponse.RefreshTokenExpiration = refreshToken.ExpirationOn.ToLocalTime();
            }

            return authenticationResponse;
        }

        public async Task<string> GenerateConfirmationEmailCodeAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = new Random().Next(100000, 999999).ToString();

            user.EmailConfirmationToken = token;
            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiresAt = DateTime.UtcNow.AddHours(24);
            await _userManager.UpdateAsync(user);

            return code;
        }

        public async Task<string> GeneratePasswordResetCodeAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var code = new Random().Next(100000, 999999).ToString();

            user.PasswordResetCode = code;
            user.PasswordResetToken = token;
            user.PasswordResetCodeExpiresAt = DateTime.UtcNow.ToLocalTime().AddHours(1);
            await _userManager.UpdateAsync(user);
            return code;
        }
    }
}
