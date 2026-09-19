using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;
namespace Donation.Application.DTOs.Request;

public sealed record LogoutRequest
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; set; }
}