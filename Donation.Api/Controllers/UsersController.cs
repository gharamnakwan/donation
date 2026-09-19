using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Donation.Application.Abstractions.Services;
using Donation.Application.DTOs.Request;

namespace Donation.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous] 
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var isSuccess = await _userService.RegisterAsync(request, isExternalUser: true, phoneNumberConfirmed: true);

        if (!isSuccess)
        {
            return BadRequest(new { message = "Registration failed. Please check your data or try again." });
        }

        return Ok(new { message = "User registered successfully." });
    }
}