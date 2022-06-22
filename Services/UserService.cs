using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data;
using Domain.DTO;
using Domain.Entities;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<T> GetDynamicAsync<T>(string endpoint);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApiService _apiService;

        public UserService(IUserRepository userRepository, IApiService apiService)
        {
            _userRepository = userRepository;
            _apiService = apiService;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            var user = await _userRepository.GetByUserNamePasswordAsync(model.UserName, model.UserName);

            if (user == null) return null;

            var token = GenerateJWTToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<T> GetDynamicAsync<T>(string endpoint)
        {
            return await _apiService.GetAsync<T>(endpoint);
        }

        private static string GenerateJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //TODO: replace with much better key and also where it is stored
            var key = Encoding.ASCII.GetBytes("ThisIsMyCustomSecretKeyAuthenticationSampleKey");

            List<Claim> claims = new();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim("valid", "1"));
            claims.Add(new Claim("userId", user.Id.ToString()));
            claims.Add(new Claim("name", $"{user.FirstName} {user.LastName}"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(30), //set to expire token for 30 seconds
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
