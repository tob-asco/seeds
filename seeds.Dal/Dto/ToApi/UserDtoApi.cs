using System.Diagnostics.CodeAnalysis;

namespace seeds.Dal.Dto.ToApi;

public class UserDtoApi
{
    public string Username { get; set; } = String.Empty;
    [AllowNull]
    public string Password { get; set; }
    [AllowNull]
    public string Email { get; set; }
}
