namespace AuthService.Data.Models;

public class AccessToken
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string Type { get; set; }
    public TimeSpan Expires { get; set; }
}
