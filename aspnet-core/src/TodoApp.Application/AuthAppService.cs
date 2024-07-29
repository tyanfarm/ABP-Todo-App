using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace TodoApp
{
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IConfiguration _configuration;
        //private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IdentityUserManager _userManager;

        public AuthAppService(
            IConfiguration configuration,
            //IIdentityUserAppService identityUserRepository,
            IdentityUserManager userManager)
        {
            _configuration = configuration;
            //_identityUserRepository = identityUserRepository;
            _userManager = userManager;
        }

        public async Task<string> LoginAsync(UserLoginDto input)
        {
            //var user = await _identityUserRepository.FindByLoginAsync(email, password);
            var user = await _userManager.FindByEmailAsync(input.Email);

            if (user == null) 
            {
                throw new Exception("Invalid username or password");
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, input.Password);

            if (checkPassword == false)
            {
                throw new Exception("Invalid credentials");
            }

            return GenerateJwtToken(user);
        }

        public async Task<IdentityUserDto> RegisterAsync(UserRegisterDto input)
        {
            var userExists = await _userManager.FindByEmailAsync(input.Email);

            if (userExists != null)
            {
                throw new Exception("Email already exists");
            }

            var newUser = new IdentityUser(Guid.NewGuid(), input.UserName, input.Email)
            {
                Name = input.UserName
            };

            var result = await _userManager.CreateAsync(newUser, input.Password);
            
            if (result.Succeeded == false)
            {
                throw new Exception(result.Errors.JoinAsString("\n"));
            }

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(newUser);
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            // SymmetricSecurityKey - khóa đối xứng
            // mã hóa bằng thuật toán HMAC-SHA256
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                // JWT ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // Issued At Time - Chứa thời điểm token được tạo
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())

            };

            var tokenDescriptor = new JwtSecurityToken
            (
                // Claims
                claims: claims,

                expires: DateTime.UtcNow
                .Add(TimeSpan
                .Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value)),

                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
