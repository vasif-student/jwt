using Authentication.Models;
using Authentication.Models.Dtos;
using DomainModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSetting _jwt;
        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtSetting> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<LoginResponseDto> Login(LoginModelDto model)
        {
            var loginResponseDto = new LoginResponseDto();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                loginResponseDto.IsAuthenticated = false;
                loginResponseDto.Message = $"No Accounts Registered with {model.Email}.";
                return loginResponseDto;
            }
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                loginResponseDto.IsAuthenticated = false;
                loginResponseDto.Message = $"Incorrect Credentials for user {user.Email}.";
                return loginResponseDto;
            }

            loginResponseDto.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            loginResponseDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            loginResponseDto.Email = user.Email;
            loginResponseDto.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            loginResponseDto.Roles = rolesList.ToList();
            return loginResponseDto;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
