using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Services;
using DeliveryFlow.Application.Contracts.Dto.UserDto;
using DeliveryFlow.Domain.Common;
using DeliveryFlow.Domain.Entities;
using DeliveryFlow.Shared.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeliveryFlow.Application.Services
{
    public class AuthenticationService : IAuthenticationUserService
    {
        private readonly IRepositoryUoW _repositoryUoW;
        private readonly UserManager<UserEntity> _userManager;

        public AuthenticationService(
          IRepositoryUoW repositoryUoW,
          UserManager<UserEntity> userManager)
        {
            _repositoryUoW = repositoryUoW;
            _userManager = userManager;
        }

        public async Task<Result<string>> Login(UserForAuthenticationDTO userEntity)
        {
            if (string.IsNullOrWhiteSpace(userEntity.UserName) || string.IsNullOrWhiteSpace(userEntity.Password))
            {
                Log.Warning(LogMessages.InvalidLoginInputs());
                return Result<string>.Error("Email and password are required.");
            }

            var response = await _repositoryUoW.UserRepository.GetByUserName(userEntity.UserName);

            if (response is null)
            {
                Log.Warning(LogMessages.InvalidLoginInputs());
                return Result<string>.Error("Invalid email or password.");
            }

            var isPasswordValid = await _repositoryUoW.UserRepository.CheckPassword(response, userEntity.Password);

            if (!isPasswordValid)
            {
                Log.Warning(LogMessages.InvalidUserInputs());
                return Result<string>.Error("Invalid email or password.");
            }

            var token = await CreateAccessTokenAsync(response);
            Log.Information(LogMessages.LoginUserSuccess(response));
            return Result<string>.Ok(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
                issuer: "PedroIghor",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );
            Log.Information(LogMessages.TokenGenerateSuccess());
            return tokenOptions;
        }
        private SymmetricSecurityKey JwtSecret() => new(Encoding.UTF8.GetBytes("EAA4Cf4JnqYwBP9MSZC8cHvMSvmShHZBU27qQxZBS3ORNSoIdEz3me0QHZABLNBiEWtDmVLZBVeMF8QZCd"));


        private async Task<string> CreateAccessTokenAsync(UserEntity user)
        {
            var signingCredentials = new SigningCredentials(JwtSecret(), SecurityAlgorithms.HmacSha256);
            var claims = await GetUserClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private async Task<List<Claim>> GetUserClaimsAsync(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? "")
            };

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            if (!string.IsNullOrWhiteSpace(user.Name))
                claims.Add(new Claim(ClaimTypes.GivenName, user.Name));

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}