using Microsoft.AspNetCore.Mvc;
using taskManager.Models;
using taskManager.Services;

using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using taskManager.Data;
namespace taskManager.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        public AuthController(DataContext context, AuthService authService, JwtService jwtService)
        {
            _context = context;
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
                return BadRequest("User already exists");

            _authService.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            Random random = new Random();
            int randomNumber = random.Next(0, 1000);
            var user = new User
            {

                Id = randomNumber,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null || !_authService.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid email or password");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
