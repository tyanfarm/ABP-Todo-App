using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using TodoApp.Dtos.Events;
using TodoApp.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace TodoApp
{
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IConfiguration _configuration;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILocalEventBus _localEventBus;

        public AuthAppService(
            IConfiguration configuration,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            //RoleManager<IdentityRole> roleManager,
            ILocalEventBus localEventBus)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            //_roleManager = roleManager;
            _localEventBus = localEventBus;
        }

        public async Task<string> LoginAsync(UserLoginDto input)
        {
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

            // PUBLISH EVENT
            await _localEventBus.PublishAsync(
                new UserEvent
                {
                    UserId = user.Id,
                    ServiceName = "UserLogin"
                }
            );

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
            // ---- Role-Based Authorization ----
            //else
            //{
            //    var role = await _roleManager.RoleExistsAsync("Client");

            //    if (!role)
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), "Client"));
            //    }

            //    await _userManager.AddToRoleAsync(newUser, "Client");
            //}
            

            // PUBLISH EVENT
            await _localEventBus.PublishAsync(
                new UserEvent
                {
                    UserId = newUser.Id,
                    ServiceName = "UserRegister"
                }
            );

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(newUser);
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHanlder = new JwtSecurityTokenHandler();

            // SymmetricSecurityKey - khóa đối xứng
            // mã hóa bằng thuật toán HMAC-SHA256
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                new Claim("Permission", TodoAppPermissions.Todo.Default),
                //new Claim(ClaimTypes.Role, "Admin"),
                // JWT ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // Issued At Time - Chứa thời điểm token được tạo
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())

            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value)),

                SigningCredentials = credentials
            };

            var token = jwtTokenHanlder.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHanlder.WriteToken(token);
            return jwtToken;
        }
    }
}
