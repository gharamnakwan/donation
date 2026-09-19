using Donation.Application.DTOs.Request;
using Donation.Application.DTOs.Response;

namespace Donation.Application.Abstractions.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    //Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}