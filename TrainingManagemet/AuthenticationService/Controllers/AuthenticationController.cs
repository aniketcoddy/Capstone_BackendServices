using AuthenticationService.Data;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(AuthenticationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Endpoint to register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            // Check if the email is already registered
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return BadRequest("Email address is already registered.");
            }

            // Hash the password before saving it
            string hashedPassword = HashPassword(model.Password);

            // Convert role name to role ID
            int roleId = GetRoleId(model.Role);

            // Create a new User object
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Role = roleId,
                Status = true,
                PasswordHash = hashedPassword
            };

            // Add user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // Endpoint to authenticate a user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            // Check if user exists, verify password, and check if status is true
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash) || !user.Status)
            {
                return Unauthorized("Invalid email or password, or user is inactive.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        // Endpoint to update a user
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateModel model)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update user fields
            user.Name = model.Name;
            user.Email = model.Email;
            user.PasswordHash = HashPassword(model.Password);
            user.Role = GetRoleId(model.Role);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("User updated successfully.");
        }

        // Endpoint to delete a user (set status to false)
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Set user status to false
            user.Status = false;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully.");
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim("name", user.Name)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                string[] parts = hashedPassword.Split(':');
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] storedHash = Convert.FromBase64String(parts[1]);

                byte[] computedHash = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);

                return storedHash.SequenceEqual(computedHash);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int GetRoleId(string role)
        {
            return role.ToLower() switch
            {
                "admin" => 1,
                "manager" => 2,
                "employee" => 3,
                _ => 0
            };
        }
    }
}
