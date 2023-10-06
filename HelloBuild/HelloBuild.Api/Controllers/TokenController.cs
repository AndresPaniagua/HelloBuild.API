using HelloBuild.Application.Services.Interfaces;
using HelloBuild.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HelloBuild.Api.Controllers
{
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Token authentication by credentials.
        /// </summary>
        /// <param name="filters">Filters to apply.</param>
        /// <returns></returns>
        /// <response code="200">Succes, retrieve the token.</response>
        /// <response code="404">No Content</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("Authentication")]
        public async Task<IActionResult> Authentication(UserExistRequest request)
        {
            if (await ValidateUser(request))
            {
                string token = GenerateToken(request.Email);
                return Ok(new { Token = token });
            }

            return BadRequest(new { errorMessage = "Invalid user" });
        }

        private async Task<bool> ValidateUser(UserExistRequest request)
        {
            Response user = await _userService.UserExist(request);
            return user.IsSuccess;
        }

        private string GenerateToken(string email)
        {
            // Header
            SymmetricSecurityKey symetricSecurityKey = new(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            SigningCredentials signingCredentials = new(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtHeader header = new(signingCredentials);

            // Claims
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, "User"),
            };

            // Payload 
            JwtPayload payload = new(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(30)
            );

            JwtSecurityToken token = new(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
