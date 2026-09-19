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

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation("Registration attempt started for email: {Email}", request.Email);

        var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingUserByEmail is not null)
        {
            throw new Exception("Email is already in use.");
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName,
            UserName = request.Username,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            throw new Exception(errors);
        }

        return new AuthResponse
        {
            AccessToken = "temp-token",
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
            RefreshToken = "temp-refresh",
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }
    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        _logger.LogInformation("Refresh token attempt started.");

        // 1. استخراج الـ Claims من الـ Access Token انتهى الصلاحية
         //var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);
        // if (principal is null)
        // {
        //     throw new Exception("Invalid access token.");
        // }

        // 2. منطق التحقق وإنشاء التوكين الجديد
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

        // منطق إبطال التوكين أو معالجته
        return await Task.FromResult(new AuthResponse
        {
            AccessToken = string.Empty,
            RefreshToken = string.Empty
        });
    }
}