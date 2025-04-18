using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class AutheService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        public AutheService(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task Sigin(SiginDTO user) {
            User userToAdd = new User()
            {
                Email = user.Email,
                Nom = user.Nom,
                Prenom = user.Prenom,
                UserName = user.Email
            };
            var result = await _userManager.CreateAsync(userToAdd,user.Password);
            if (result.Succeeded)
            {
                return;
            }
            else
            {
                throw new BadHttpRequestException(result.Errors.FirstOrDefault().Description);
            }
        }
        public async Task RoleAsync(RoleDTO role)
        {
            Role addRoles = new Role()
            {
                Name = role.Nom,
                Description = role.Description
            };
            var result = await _roleManager.CreateAsync(addRoles);
            if (result.Succeeded)
            {
                return;
            }
            else
            {
                throw new BadHttpRequestException(result.Errors.FirstOrDefault().Description);
            }
        }

        public async Task<string> Login(LoginDTO login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var token = GenerateJwtToken(user);
                return token;
            }
            else
            {
                throw new UnauthorizedAccessException($"Failed to login{login.Email}");
            }
        }

        public string GenerateJwtToken(User user) 
        {
            var jwtSettings = _configuration.GetSection("jwt");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}