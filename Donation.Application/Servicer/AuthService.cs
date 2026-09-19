using Donation.Application.Abstractions.Services;
using Donation.Application.DTOs.Request;
using Donation.Application.DTOs.Response;
using Donation.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Donation.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(
        ILogger<AuthService> logger,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        _logger.LogInformation("Refresh token attempt started.");

        return await Task.FromResult(new AuthResponse
        {
            AccessToken = "new-access-token",
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
            RefreshToken = "new-refresh-token",
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
        });
    }

    public async Task<AuthResponse> LogoutAsync(LogoutRequest request)
    {
        _logger.LogInformation("Logout attempt started.");

        return await Task.FromResult(new AuthResponse
        {
            AccessToken = string.Empty,
            RefreshToken = string.Empty
        });
    }
}