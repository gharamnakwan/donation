namespace Donation.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;

    // Navigation Property نظيفة بين كينونات الـ Domain فقط
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}