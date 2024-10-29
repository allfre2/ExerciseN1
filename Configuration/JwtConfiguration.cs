namespace AuthService.Configuration;

public class JwtConfiguration
{
    public const string Section = "Jwt";

    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int TokenExpirationMinutes { get; set; }
}
