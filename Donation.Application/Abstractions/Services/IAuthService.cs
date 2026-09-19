using Donation.Application.DTOs.Request;
using Donation.Application.DTOs.Response;

namespace Donation.Application.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse>LogoutAsync(LogoutRequest request);

    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
}