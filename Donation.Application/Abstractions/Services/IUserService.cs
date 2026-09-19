using Donation.Application.DTOs.Request;

namespace Donation.Application.Abstractions.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(RegisterRequest request, bool isExternalUser = false, bool phoneNumberConfirmed = false);
}