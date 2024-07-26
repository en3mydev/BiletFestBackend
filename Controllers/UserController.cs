using BiletFest.Models;
using BiletFest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiletFest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BiletFestServices _biletFestServices;
        private readonly IConfiguration _configuration;

        public UserController(BiletFestServices biletFestServices, IConfiguration configuration)
        {
            _biletFestServices = biletFestServices;
            _configuration = configuration;
        }

        [HttpGet("GetAllUsers")]
        public IEnumerable<User> Get()
        {
            return _biletFestServices.GetAll();
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _biletFestServices.GetUserDataAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            bool result = await _biletFestServices.CreateUser(request.Password, request.Email, request.FullName);
            if (result)
            {
                return Ok("Contul a fost creat cu succes!");
            }
            else
            {
                return BadRequest("Email-ul exista deja.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(BiletFest.Models.LoginRequest request)
        {
            var user = await _biletFestServices.LoginAsync(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID");
            return Ok(new { message = "Logout successful." });
        }

        [HttpPut("MakeUserAdmin")]
        public IActionResult Put(int id)
        {
            _biletFestServices.MakeUserAdmin(id);
            return Ok();
        }

    }
}
