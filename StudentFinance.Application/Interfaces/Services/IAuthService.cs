using StudentFinance.Application.DTOs.Auth;

namespace StudentFinance.Application.Interfaces.Services
{
    public interface IAuthService
    {
        // Register a new user
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

        // Authenticate a user and return a JWT token
        Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
