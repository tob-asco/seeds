using System.Diagnostics.CodeAnalysis;

namespace seeds.Dal.Dto.ToAndFromDb;

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    [AllowNull]
    public string Password { get; set; }
    [AllowNull]
    public string Email { get; set; }
}
