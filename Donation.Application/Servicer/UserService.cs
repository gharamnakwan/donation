using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Donation.Application.Abstractions.Services;
using Donation.Application.DTOs.Request;
using Donation.Domain.Entities;

namespace Donation.Infrastructure.Services;

public sealed class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(ILogger<UserService> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request, bool isExternalUser = false, bool phoneNumberConfirmed = false)
    {
        _logger.LogInformation("Registration attempt started for username: {Username}", request.Username);

        var usernameExists = await _userManager.Users
            .AnyAsync(x => x.UserName == request.Username);

        if (usernameExists)
        {
            _logger.LogWarning("Registration failed: Username already exists - {Username}", request.Username);
            return false;
        }
        var emailExists = await _userManager.Users
            .AnyAsync(x => x.Email == request.Email);

        if (emailExists)
        {
            _logger.LogWarning("Registration failed: Email already exists - {Email}", request.Email);
            return false;
        }
        var user = new ApplicationUser
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Username,
            PhoneNumber = request.PhoneNumber,
            PhoneNumberConfirmed = phoneNumberConfirmed
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(x => x.Description));
            _logger.LogWarning("Registration failed for {Username}. Errors: {Errors}", request.Username, errors);
            return false;
        }
        //var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

        //if (!roleResult.Succeeded)
        //{
        //    var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));
        //    _logger.LogError("Failed to assign role to user {UserId}. Errors: {Errors}", user.Id, errors);
        //    return false;
        //}

        _logger.LogInformation("User successfully registered with default 'Admin' role: {UserId}", user.Id);
        return true;
    }
}