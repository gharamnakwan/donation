using Donation.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }

    public Guid UserId { get; set; }
    // غيري ApplicationUser User إلى User User هنا:
    public User User { get; set; } = null!;
}