using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentFinance.Application.DTOs.Auth;
using StudentFinance.Application.Interfaces.Services;

namespace StudentFinance.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (result.ErrorMessage != null)
                return BadRequest(new { message = result.ErrorMessage });

            return Created("", new { message = "User registered successfully." });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var tokenResponse = await _authService.LoginAsync(request, cancellationToken);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (tokenResponse == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(tokenResponse); // return JWT Token
        }
    }
}
