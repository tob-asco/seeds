using System.Diagnostics.CodeAnalysis;

namespace seeds.Dal.Dto.FromDb;

public class UserFromDb
{
    public string Username { get; set; } = string.Empty;
    [AllowNull]
    public string Password { get; set; }
    [AllowNull]
    public string Email { get; set; }
}
